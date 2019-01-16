using StructureMap;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();

            For<EmployerFinanceConfiguration>().Use(c => c.GetInstance<ITableStorageConfigurationService>().Get<EmployerFinanceConfiguration>(ConfigurationKeys.EmployerFinance)).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<EmployerFinanceConfiguration>().EmployerUrls).Singleton();
            For<IOidcConfiguration>().Use(c => c.GetInstance<EmployerFinanceConfiguration>().Oidc).Singleton();
        }
    }
}