using Microsoft.AspNetCore.Hosting;
using StructureMap;
using SFA.DAS.EmployerFinance.Environment;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    //rename DasNonMvcHostingEnvironmentRegistry
    public class NonMvcHostingEnvironmentRegistry : Registry
    {
        public NonMvcHostingEnvironmentRegistry(string environmentName)
        {
            For<IHostingEnvironment>().Use(() => DasHostingEnvironment.Create(environmentName)).Singleton();
        }
    }
}