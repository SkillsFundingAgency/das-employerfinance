using SFA.DAS.EmployerFinance.DependencyResolution;
using IContainer = StructureMap.IContainer;
using Container = StructureMap.Container;

namespace SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
