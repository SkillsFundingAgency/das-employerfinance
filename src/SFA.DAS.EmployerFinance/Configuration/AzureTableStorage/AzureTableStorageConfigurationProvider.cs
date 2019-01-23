using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    //todo: have config types and config code in different folders/namespaces
    //todo: pick up storage connection string from env variables storage provider in here
    //todo: implement reload on change is table supports it
    //todo: inject config into views??
    //todo: how to handle which rows to load? supply collection of names that apply (if we do that, it'll mean 3 tiered config, row/config group/config item, which would fit into section/subsection/key ok)?
    // base AzureTableStorageConfigurationProvider and derive from it with row name? or do we load all from 1 row?? some other mechanism? 
    //todo: options??
    //todo: async load??
    //todo: use new table code in cosmos package ?? not currently an option: https://github.com/Azure/azure-cosmos-dotnet-v2/issues/344
    // ^^ could use this preview package: https://www.nuget.org/packages/Microsoft.Azure.Cosmos.Table/0.10.1-preview
    //todo: getsection, then bind to poco and put in container, or let everyone work with iconfiguration?
    /// <remarks>
    /// Inspired by...
    /// https://github.com/SkillsFundingAgency/das-reservations/blob/MF-7-reservations-web/src/SFA.DAS.Reservations.Infrastructure/Configuration/AzureTableStorageConfigurationProvider.cs
    /// </remarks>
    public class AzureTableStorageConfigurationProvider : ConfigurationProvider
    {
        // das's tools (das-employer-config) don't currently support different versions, so might as well hardcode it
        // we provider versioning by appending a 'Vn' on to the name
        private const string Version = "1.0";
        private readonly IEnumerable<string> _rowNames;
        private readonly string _environment;
        private readonly CloudStorageAccount _storageAccount;

        public AzureTableStorageConfigurationProvider(string connection, string environment, IEnumerable<string> rowNames)
        {
            _rowNames = rowNames;
            _environment = environment;
            _storageAccount = CloudStorageAccount.Parse(connection);
        }

        private class ConfigurationItem : TableEntity
        {
            public string Data { get; set; }
        }
        
        public override void Load()
        {
            var table = GetTable();

            var operations = _rowNames.Select(rn => table.ExecuteAsync(GetOperation(rn)));

            var rows = Task.WhenAll(operations).GetAwaiter().GetResult();

            var rowsConfigItems = rows.Select(r => r.Result).Cast<ConfigurationItem>();

            var rowsKeyValues = rowsConfigItems.Select(ci => JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(ci.Data));

            //var flattened = rowsKeyValues.SelectMany(rkv => rkv, (p, c) => c);
            
//            _rowNames.Zip(rowConfigItems, (rn, config) => $"{rn}:{config.Data}")
            //_rowNames.Zip(rowsKeyValues, (rn, keyValues) => keyValues.SelectMany(kv => kv, (kvp, )))

            var yy = _rowNames.Zip(rowsKeyValues, GenerateFlattenedConfig);
            
            //Data = yy.SelectMany(x => x)
            
            //var operation = GetOperation(_name, _environment);
            //var result = table.ExecuteAsync(operation).Result;

            //var result = table.ExecuteAsync(operation).GetAwaiter().GetResult();
            //var configItem = (ConfigurationItem)result.Result;

            //Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(configItem.Data);            
        }

        private IEnumerable<(string, string)> GenerateFlattenedConfig(string rowName, List<KeyValuePair<string, string>> keyValues)
        {
            return keyValues.Select(kv => ($"{rowName}:{kv.Key}", kv.Value));
        }

        private CloudTable GetTable()
        {
            var tableClient = _storageAccount.CreateCloudTableClient();
            return tableClient.GetTableReference("Configuration");
        }

        private TableOperation GetOperation(string serviceName)
        {
            return TableOperation.Retrieve<ConfigurationItem>(_environment, $"{serviceName}_{Version}");
        }
    }
}