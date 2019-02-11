using System.Net;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution;
using SFA.DAS.EmployerFinance.MessageHandlers.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers
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