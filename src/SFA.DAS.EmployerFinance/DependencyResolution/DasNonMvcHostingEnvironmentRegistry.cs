using Microsoft.AspNetCore.Hosting;
using StructureMap;
using SFA.DAS.EmployerFinance.Environment;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class DasNonMvcHostingEnvironmentRegistry : Registry
    {
        public DasNonMvcHostingEnvironmentRegistry(string environmentName)
        {
            For<IHostingEnvironment>().Use(() => DasHostingEnvironment.Create(environmentName)).Singleton();
        }
    }
}