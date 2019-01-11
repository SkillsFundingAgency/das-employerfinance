using SFA.DAS.EmployerFinance.DependencyResolution;
using IContainer = StructureMap.IContainer;
using Container = StructureMap.Container;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<DefaultRegistry>();
                c.AddRegistry<LoggingRegistry>();
            });
        }
    }
}
