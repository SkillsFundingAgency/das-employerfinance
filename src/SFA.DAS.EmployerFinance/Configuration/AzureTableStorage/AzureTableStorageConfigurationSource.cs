using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    public class AzureTableStorageConfigurationSource : IConfigurationSource
    {
        private readonly string _connection;
        private readonly string _environment;
        private readonly IEnumerable<AzureTableStorageConfigurationDescriptor> _configDescriptors;

        public AzureTableStorageConfigurationSource(string connection, string environment, IEnumerable<AzureTableStorageConfigurationDescriptor> configDescriptors)
        {
            _connection = connection;
            _environment = environment;
            _configDescriptors = configDescriptors;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            //todo: something that returns json files, then add jsonconfigurationsource, then don't need to add AzureTableStorageConfigurationProvider?
            return new AzureTableStorageConfigurationProvider(builder, _connection, _environment, _configDescriptors);
        }
    }
}