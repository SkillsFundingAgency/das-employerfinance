using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration.AzureTableStorage
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSourcedAzureTableStorageConfiguration(this IConfigurationBuilder builder,
            string connection, string environment, params string[] configurationKeys)
        {
            return builder.Add(new AzureTableStorageConfigurationSource(connection, environment, configurationKeys));
        }

        public static IConfigurationBuilder AddAzureTableStorageConfiguration(this IConfigurationBuilder builder, params string[] configurationKeys)
        {
            var environmentVariables = ConfigurationBootstrapper.GetEnvironmentVariables();

            return AddSourcedAzureTableStorageConfiguration(builder, environmentVariables.StorageConnectionString,
                environmentVariables.EnvironmentName, configurationKeys);
        }
    }
}