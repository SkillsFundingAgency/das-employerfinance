using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(IConfiguration config, string environmentName)
        {
            return new Container(c =>
            {
                c.AddRegistry(new NonMvcHostingEnvironmentRegistry(environmentName));
                c.AddRegistry(new ConfigurationRegistryCore(config));
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
