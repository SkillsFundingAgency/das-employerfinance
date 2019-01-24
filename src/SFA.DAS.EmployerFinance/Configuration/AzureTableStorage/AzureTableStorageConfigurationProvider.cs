using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    //todo: have config types and config code in different folders/namespaces
    //todo: pick up storage connection string from env variables storage provider in here. pick up environment from core's env -> IHostingEnvironment.EnvironmentName can get to it so early?
    //todo: implement reload on change is table supports it
    //todo: inject config into views??
    //todo: how to handle which rows to load? supply collection of names that apply (if we do that, it'll mean 3 tiered config, row/config group/config item, which would fit into section/subsection/key ok)?
    // base AzureTableStorageConfigurationProvider and derive from it with row name? or do we load all from 1 row?? some other mechanism? 
    //todo: options??
    //todo: async load??
    //todo: use new table code in cosmos package ?? not currently an option: https://github.com/Azure/azure-cosmos-dotnet-v2/issues/344
    // ^^ could use this preview package: https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table/0.10.1-preview
    //todo: getsection, then bind to poco and put in container, or let everyone work with iconfiguration?
    //todo: move this provider into a hosting startup (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-2.2)
    /// <remarks>
    /// Inspired by...
    /// https://github.com/SkillsFundingAgency/das-reservations/blob/MF-7-reservations-web/src/SFA.DAS.Reservations.Infrastructure/Configuration/AzureTableStorageConfigurationProvider.cs
    /// </remarks>
    public class AzureTableStorageConfigurationProvider : ConfigurationProvider
    {
        // das's tools (das-employer-config) don't currently support different versions, so might as well hardcode it
        // we provider versioning by appending a 'Vn' on to the name
        private const string Version = "1.0";
        private readonly IConfigurationBuilder _builder;
        private readonly IEnumerable<AzureTableStorageConfigurationDescriptor> _configDescriptors;
        private readonly string _environment;
        private readonly CloudStorageAccount _storageAccount;

        public AzureTableStorageConfigurationProvider(IConfigurationBuilder builder, string connection, string environment, IEnumerable<AzureTableStorageConfigurationDescriptor> configDescriptors)
        {
            _builder = builder;
            _configDescriptors = configDescriptors;
            _environment = environment;
            _storageAccount = CloudStorageAccount.Parse(connection);
        }

        private class ConfigurationRow : TableEntity
        {
            public string Data { get; set; }
        }
        
        public override void Load()
        {
            var table = GetTable();

            var operations = _configDescriptors.Select(cd => table.ExecuteAsync(GetOperation(cd.Name)));

            var rows = Task.WhenAll(operations).GetAwaiter().GetResult();

            //var configJson = rows.Select(r => r.Result).Cast<ConfigurationRow>().Select(cr => cr.Data);
            var configJsons = rows.Select(r => ((ConfigurationRow)r.Result).Data);

            // given file paths, not json!
//            foreach (var json in configsJsons)
//            {
//                _builder.AddJsonFile(json);
//            }

            IEnumerable<Stream> configStreams = null;
            try
            {
                configStreams = configJsons.Select(GenerateStreamFromString);

                //todo: tuple
                var configNameAndStreams = _configDescriptors.Zip(configStreams,
                    (cd, stream) => new KeyValuePair<string, Stream>(cd.Name, stream));

                //todo: selectmany?
                foreach (var kvp in configNameAndStreams)
                {
                    //todo: can access public static in internal file? if so use original
                    var configData = JsonConfigurationStreamParser.Parse(kvp.Value);

                    foreach (var configItem in configData)
                        Data.Add($"{kvp.Key}:{configItem.Key}", configItem.Value);
                }
            }
            finally
            {
                foreach (var stream in configStreams)
                {
                    stream.Dispose();
                }
            }

//            _configDescriptors.Zip(configJsons, (cd, json) => new KeyValuePair<string,string>())
//
//            foreach (var json in configJsons)
//            {
//                //todo: can access public static in internal file? if so use original
//                using (var stream = GenerateStreamFromString(json))
//                {
//                    var data = JsonConfigurationStreamParser.Parse(stream);
//
//                    foreach (var kvp in data)
//                        Data.Add($":{kvp.Key}", kvp.Value);
//                }
//            }
            

            //var configObjects = configsJsons.Zip(_configDescriptors, (json, desc) => JsonConvert.DeserializeObject(json, desc.Type));

            //pass through IConfigurationBuilder properties

            //we're before StructureMap setup, so...

            // we either can convert poco's to individual config files, add to config, then in configureservices convert them back and add them using Configuration.GetSection
            // -ve: convert back and fore unnecessarily
            // +ve: set up services in normal location, can pick out subsections easily and add them too

            //todo: if we need facility to override config from e.g. command line, env variable (so can inject devs own connection string etc), then can stuff config into IOptions
            //todo: how to handle sub objects? want code to only inject sub object, but how to handle that best??
        }

        public static Stream GenerateStreamFromString(string s)
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
            return tableClient.GetTableReference("Configuration");
        }

        private TableOperation GetOperation(string serviceName)
        {
            return TableOperation.Retrieve<ConfigurationRow>(_environment, $"{serviceName}_{Version}");
        }
    }
}