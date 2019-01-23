using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    public class AzureTableStorageConfigurationSource : IConfigurationSource
    {
        private readonly string _connection;
        private readonly string _environment;
        private readonly IEnumerable<string> _rowNames;

        public AzureTableStorageConfigurationSource(string connection, string environment, IEnumerable<string> rowNames)
        {
            _connection = connection;
            _environment = environment;
            _rowNames = rowNames;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AzureTableStorageConfigurationProvider(_connection, _environment, _rowNames);
        }
    }
}