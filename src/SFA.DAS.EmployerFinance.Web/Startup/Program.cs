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

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddAzureTableStorageConfiguration(
                        new[] 
                        {
                            ConfigurationKeys.EmployerFinance, 
                            Api.Client.Configuration.ConfigurationKeys.ApiClient
                        });
                })
                .UseKestrel(options => options.AddServerHeader = false)
                .UseNLog()
                .UseStartup<Startup>()
                .UseStructureMap();
        }
    }
}