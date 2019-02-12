using System.Net;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Jobs.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static void Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
            
            var host = new HostBuilder()
                .ConfigureDasWebJobs()
                .ConfigureDasAppConfiguration()
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseStructureMap()
                .UseConsoleLifetime()
                .ConfigureServices(s => s.AddDasNServiceBus())
                .ConfigureContainer<Registry>(IoC.Initialize)
                .Build();
            
            using (host)
            {
                host.Run();
            }
        }
    }
}