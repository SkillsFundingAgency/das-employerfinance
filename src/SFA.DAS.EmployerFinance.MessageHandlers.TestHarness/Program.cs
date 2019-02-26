using System;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.DependencyResolution;
using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args) => 
            new HostBuilder()
                .ConfigureDasAppConfiguration(args)
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseStructureMap()
                .UseConsoleLifetime()
                .ConfigureServices(s => s.AddDasNServiceBus())
                .ConfigureContainer<Registry>(IoC.Initialize);
    }
}