using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<AutoConfigurationRegistry>();
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<LoggingRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}