using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry, IConfiguration configuration)
        {
            registry.IncludeRegistry(new ConfigurationRegistry(configuration));
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}