﻿using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution;
using SFA.DAS.EmployerFinance.MessageHandlers.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers
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
                .UseApplicationInsights()
                .UseDasEnvironment()
                .UseStructureMap()
                .UseConsoleLifetime()
                .ConfigureServices(s => s.AddDasNServiceBus())
                .ConfigureContainer<Registry>(IoC.Initialize);
    }
}