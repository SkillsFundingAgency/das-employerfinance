using System;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public class ConfigurationBootstrapper
    {
        private const string DeveloperEnvironment = "LOCAL";
        private const string DeveloperEnvironmentDefaultStorageConnectionString = "UseDevelopmentStorage=true";

        public static (string StorageConnectionString, string EnvironmentName) GetEnvironmentVariables()
        {
            var environmentVariablesConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();

            var environmentName = environmentVariablesConfig[EnvironmentVariableNames.Environment] ?? DeveloperEnvironment;
            
            var storageConnectionString = environmentVariablesConfig[EnvironmentVariableNames.ConfigurationStorageConnectionString];
            if (string.IsNullOrWhiteSpace(storageConnectionString))
            {
                if (string.Equals(environmentName, DeveloperEnvironment, StringComparison.OrdinalIgnoreCase))
                {
                    storageConnectionString = DeveloperEnvironmentDefaultStorageConnectionString;
                }
                else
                {
                    throw new Exception($"Missing environment variable '{EnvironmentVariableNames.ConfigurationStorageConnectionString}'. It should be present and set to a connection string pointing to the storage account containing a 'Configuration' table.");
                }
            }

            return (storageConnectionString, environmentName);
        }

        public static IConfigurationRoot GetConfiguration(string storageConnectionString, string environmentName, params string[] configurationKeys)
        {
            return new ConfigurationBuilder().AddSourcedAzureTableStorageConfiguration(
                storageConnectionString, environmentName, configurationKeys).Build();
        }
    }
}