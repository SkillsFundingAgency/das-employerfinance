using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;
using SFA.DAS.EmployerFinance.Web.Startup;

namespace SFA.DAS.EmployerFinance.Web
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
                .ConfigureAppConfiguration(c =>
                {
                    c.AddAzureTableStorageConfiguration(EmployerFinanceConfigurationKeys.Base, EmployerFinanceConfigurationKeys.ApiClient);
                })
                .UseKestrel(o => o.AddServerHeader = false)
                .UseNLog()
                .UseStartup<AspNetCoreStartup>();
        }
    }
}