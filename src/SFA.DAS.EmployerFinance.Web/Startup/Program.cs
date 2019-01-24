using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        // use hostingContext.HostingEnvironment.EnvironmentName?
                        config.AddAzureTableStorageConfiguration(
                            // connection string will be picked up from an environment variable
                            "insert connection string here",
                            "LOCAL", new[] {new AzureTableStorageConfigurationDescriptor("SFA.DAS.EmployerFinanceV2", typeof (EmployerFinanceConfiguration))});
                    })
                .UseKestrel(options => options.AddServerHeader = false)
                .UseStartup<Startup>()
                .UseStructureMap();
    }
}