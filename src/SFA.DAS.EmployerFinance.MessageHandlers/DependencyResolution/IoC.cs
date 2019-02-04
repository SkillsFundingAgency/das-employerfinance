using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(IConfiguration config, string environmentName)
        {
            return new Container(c =>
            {
                c.AddRegistry(new ConfigurationRegistry(config));
                c.AddRegistry(new DasNonMvcHostingEnvironmentRegistry(environmentName));
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
