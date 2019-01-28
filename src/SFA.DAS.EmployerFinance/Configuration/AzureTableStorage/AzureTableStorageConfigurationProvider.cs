using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    //todo: das-recruit just picks up the values straight from environment variables (which devops have to set anyway). we could too and cut out all this table reading provider code etc.
    //todo: have config types and config code in different folders/namespaces
    //todo: implement reload on change if table supports it
    //todo: inject config into views??
    //todo: options??
    //todo: use new table code in cosmos package ?? not currently an option: https://github.com/Azure/azure-cosmos-dotnet-v2/issues/344
    // ^^ could use this preview package: https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table/0.10.1-preview
    //todo: move this provider into a hosting startup (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-2.2)
    //todo: if we need facility to override config from e.g. command line, env variable (so can inject devs own connection string etc), then can stuff config into IOptions
    /// <remarks>
    /// Inspired by...
    /// https://github.com/SkillsFundingAgency/das-reservations/blob/MF-7-reservations-web/src/SFA.DAS.Reservations.Infrastructure/Configuration/AzureTableStorageConfigurationProvider.cs
    /// </remarks>
    public class AzureTableStorageConfigurationProvider : ConfigurationProvider
    {
        // das's tools (das-employer-config) don't currently support different versions, so might as well hardcode it
        // we provider versioning by appending a 'Vn' on to the name
        private const string Version = "1.0";
        private const string ConfigurationTableName = "Configuration";
        private readonly IEnumerable<string> _configNames;
        private readonly string _environment;
        private readonly CloudStorageAccount _storageAccount;

        public AzureTableStorageConfigurationProvider(string connection, string environment, IEnumerable<string> configNames)
        {
            _configNames = configNames;
            _environment = environment;
            _storageAccount = CloudStorageAccount.Parse(connection);
        }

        private class ConfigurationRow : TableEntity
        {
            public string Data { get; set; }
        }
        
        public override void Load()
        {
            var configJsons = GetRows().Select(r => ((ConfigurationRow)r.Result).Data);

            IEnumerable<Stream> configStreams = null;
            try
            {
                configStreams = configJsons.Select(GenerateStreamFromString);

                var configNameAndStreams = _configNames.Zip(configStreams, (name, stream) => (name, stream));

                Parallel.ForEach(configNameAndStreams, AddToData);
            }
            finally
            {
                foreach (var stream in configStreams)
                {
                    stream.Dispose();
                }
            }
        }

        /// <returns>Stream that contains the supplied string. The caller is responsible for disposing the stream.</returns>
        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        
        private CloudTable GetTable()
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference(ConfigurationTableName);
        }

        private IEnumerable<TableResult> GetRows()
        {
            var table = GetTable();
            var operations = _configNames.Select(name => table.ExecuteAsync(GetOperation(name)));
            return Task.WhenAll(operations).GetAwaiter().GetResult();
        }

        private TableOperation GetOperation(string serviceName)
        {
            return TableOperation.Retrieve<ConfigurationRow>(_environment, $"{serviceName}_{Version}");
        }
        
        // return projection, then add that to data instead?
        private void AddToData((string name, Stream stream) configNameAndStream)
        {
            var configData = JsonConfigurationStreamParser.Parse(configNameAndStream.stream);

            foreach (var configItem in configData)
                Data.Add($"{configNameAndStream.name}:{configItem.Key}", configItem.Value);
        }
    }
}