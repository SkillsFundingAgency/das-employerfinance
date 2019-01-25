using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<EmployerFinanceConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<EmployerFinanceConfiguration>(ConfigurationKeys.EmployerFinance)).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<EmployerFinanceConfiguration>().EmployerUrls).Singleton();
            For<IOidcConfiguration>().Use(c => c.GetInstance<EmployerFinanceConfiguration>().Oidc).Singleton();
        }
    }
}