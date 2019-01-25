using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Api.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}