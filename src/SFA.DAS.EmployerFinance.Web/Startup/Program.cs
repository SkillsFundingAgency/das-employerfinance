using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using NLog.Web;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var environmentVariables = ConfigurationBootstrapper.GetEnvironmentVariables();
            //todo: pick up the environment also, and integrate it into core's environment system
            
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // use hostingContext.HostingEnvironment.EnvironmentName?
                    config.AddAzureTableStorageConfiguration(
                        environmentVariables.StorageConnectionString,
                        environmentVariables.EnvironmentName, new[] {ConfigurationKeys.EmployerFinance});
                })
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseStructureMap()
                .UseNLog();
        }
    }
}