using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddAzureTableStorageConfiguration(this IConfigurationBuilder builder, string connection, string environment, IEnumerable<AzureTableStorageConfigurationDescriptor> configDescriptors)
        {
            return builder.Add(new AzureTableStorageConfigurationSource(connection, environment, configDescriptors));
        }
    }
}