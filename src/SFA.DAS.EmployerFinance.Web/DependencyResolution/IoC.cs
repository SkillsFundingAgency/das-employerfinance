using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
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