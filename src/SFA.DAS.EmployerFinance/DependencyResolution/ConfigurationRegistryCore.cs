using System.Linq;
using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public static class ConfigurationExtensions
    {
        public static TConfig GetEmployerFinanceSection<TConfig>(this IConfiguration configuration, params string[] subSectionPath)
        {
            return configuration.GetSection(string.Join(":", Enumerable.Repeat(ConfigurationKeys.EmployerFinance, 1).Concat(subSectionPath))).Get<TConfig>();
        }
    }
    
    //todo: rename to ConfigurationRegistry, once there are no consumers of the old registry
    public class ConfigurationRegistryCore : Registry
    {
        public ConfigurationRegistryCore(IConfiguration configuration)
        {
            // either get root and pick from there, or get individually
            // todo: don't have any values in root config, then won't rehydrate twice and will only rehydrate what gets injected
            For<EmployerFinanceConfiguration>().Use(() => configuration.GetEmployerFinanceSection<EmployerFinanceConfiguration>()).Singleton();
            For<IEmployerUrlsConfiguration>().Use(() => configuration.GetEmployerFinanceSection<EmployerUrlsConfiguration>("EmployerUrls")).Singleton();
            For<IOidcConfiguration>().Use(() => configuration.GetEmployerFinanceSection<OidcConfiguration>("Oidc")).Singleton();
        }
    }
}
    