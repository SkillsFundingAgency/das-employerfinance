using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IHostingEnvironment>().Use<HostingEnvironmentAdapter>();
        }
    }
}