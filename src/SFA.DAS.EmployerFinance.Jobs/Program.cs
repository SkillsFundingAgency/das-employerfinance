using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Jobs.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => 
            new HostBuilder()
                .ConfigureDasWebJobs()
                .ConfigureDasAppConfiguration(args)
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseStructureMap()
                .UseConsoleLifetime()
                .ConfigureServices(s => s.AddDasNServiceBus())
                .ConfigureContainer<Registry>(IoC.Initialize);
    }
}