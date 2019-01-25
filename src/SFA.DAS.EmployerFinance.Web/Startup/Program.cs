using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class EnvironmentVariableNames
    {
        public const string Environment = "EnvironmentName";
        public const string ConfigurationStorageConnectionString = "ConfigurationStorageConnectionString";
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            var environmentVariablesConfig = builder.Build();
            var storageConnectionString = environmentVariablesConfig[EnvironmentVariableNames.ConfigurationStorageConnectionString];
            if (string.IsNullOrWhiteSpace(storageConnectionString))
                throw new Exception($"Hey {environmentVariablesConfig["USERNAME"]??"developer"}, you need to set the environment variable '{EnvironmentVariableNames.ConfigurationStorageConnectionString}'. Set it to a connection string pointing to a storage account containing a 'Configuration' table. See readme.md for more information.");
                
            //todo: pick up the environment also, and integrate it into core's environment system
            
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // use hostingContext.HostingEnvironment.EnvironmentName?
                    config.AddAzureTableStorageConfiguration(
                        storageConnectionString,
                        "LOCAL", new[] {"SFA.DAS.EmployerFinanceV2"});
                })
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseStructureMap();
        }
    }
}