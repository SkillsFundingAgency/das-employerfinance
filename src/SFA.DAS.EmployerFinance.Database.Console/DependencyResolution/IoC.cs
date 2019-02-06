using SFA.DAS.AutoConfiguration.DependencyResolution;
using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Database.Console.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                c.AddRegistry<AutoConfigurationRegistry>();
                c.AddRegistry<LoggingRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
