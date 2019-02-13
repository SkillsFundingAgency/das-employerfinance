using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<EmployerFinanceConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<EmployerFinanceConfiguration>()).Singleton();
            For<GoogleAnalyticsConfiguration>().Use(c => c.GetInstance<IGoogleAnalyticsConfigurationFactory>().CreateConfiguration()).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<EmployerUrlsConfiguration>("EmployerUrls")).Singleton();
            For<IOidcConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<OidcConfiguration>("Oidc")).Singleton();
            For<IGoogleAnalyticsConfigurationFactory>().Use<GoogleAnalyticsConfigurationFactory>();
        }
    }
}
    