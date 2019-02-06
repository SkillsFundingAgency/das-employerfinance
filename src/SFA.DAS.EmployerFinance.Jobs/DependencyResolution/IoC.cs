using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(string environmentName, IConfiguration configuration)
        {
            return new Container(c =>
            {
                c.For<IConfiguration>().Use(configuration).Singleton();
                c.AddRegistry(new DasNonMvcHostingEnvironmentRegistry(environmentName));
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
