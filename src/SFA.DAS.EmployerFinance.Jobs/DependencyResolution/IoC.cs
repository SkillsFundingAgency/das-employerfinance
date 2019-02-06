using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(string environmentName)
        {
            return new Container(c =>
            {
                c.AddRegistry(new DasNonMvcHostingEnvironmentRegistry(environmentName));
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
