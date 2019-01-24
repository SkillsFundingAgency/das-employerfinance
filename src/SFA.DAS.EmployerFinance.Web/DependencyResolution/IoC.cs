using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry, IConfiguration configuration)
        {
            //todo: belongs in ConfigurationRegistry
            //todo: indirect, so only fetch if injected?
            var oidcConfig = configuration.GetSection($"{"SFA.DAS.EmployerFinanceV2"}:{"Oidc"}").Get<OidcConfiguration>();
            registry.For<IOidcConfiguration>().Use(oidcConfig).Singleton();

            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}