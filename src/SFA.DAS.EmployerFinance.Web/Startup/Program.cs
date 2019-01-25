using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class Program
    {
        private const string DefaultEnvironment = "LOCAL";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environmentVariablesConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();
            var storageConnectionString = environmentVariablesConfig[EnvironmentVariableNames.ConfigurationStorageConnectionString];

            if (string.IsNullOrWhiteSpace(storageConnectionString))
                throw new Exception($"Hey {environmentVariablesConfig["USERNAME"]??"developer"}, you need to set the environment variable '{EnvironmentVariableNames.ConfigurationStorageConnectionString}'. Set it to a connection string pointing to a storage account containing a 'Configuration' table. See readme.md for more information.");

            var environmentName = environmentVariablesConfig[EnvironmentVariableNames.Environment] ?? DefaultEnvironment;
            //todo: LOCAL / development etc. how to reconcile?
            // only care about legacy envnames like LOCAL, for config?
            
            //todo: pick up the environment also, and integrate it into core's environment system
            
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // use hostingContext.HostingEnvironment.EnvironmentName?
                    config.AddAzureTableStorageConfiguration(
                        storageConnectionString,
                        environmentName, new[] {ConfigurationKeys.EmployerFinance});
                })
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseStructureMap();
        }
    }
}