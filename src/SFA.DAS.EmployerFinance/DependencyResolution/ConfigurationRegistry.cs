using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.Extensions;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            //todo: once fix jobs startup, check these will be injected properly
            // either get root and pick from there, or get individually
            // todo: don't have any values in root config, then won't rehydrate twice and will only rehydrate what gets injected
            For<EmployerFinanceConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<EmployerFinanceConfiguration>()).Singleton();
            For<IEmployerUrlsConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<EmployerUrlsConfiguration>("EmployerUrls")).Singleton();
            For<IOidcConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceSection<OidcConfiguration>("Oidc")).Singleton();
            For<IGoogleAnalyticsConfigurationFactory>().Use<GoogleAnalyticsConfigurationFactory>();
            For<GoogleAnalyticsConfiguration>().Use(c => c.GetInstance<IGoogleAnalyticsConfigurationFactory>().CreateConfiguration()).Singleton();
        }
    }
}
    