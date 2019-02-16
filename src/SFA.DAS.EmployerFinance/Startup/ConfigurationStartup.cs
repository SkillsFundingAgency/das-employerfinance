using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class ConfigurationStartup
    {
        public static IHostBuilder ConfigureDasAppConfiguration(this IHostBuilder builder, string[] args)
        {
            return builder.ConfigureAppConfiguration((c, b) => b
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{c.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .AddAzureTableStorage(EmployerFinanceConfigurationKeys.Base));
        }
        
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c
                .AddAzureTableStorage(
                    EmployerFinanceConfigurationKeys.Base,
                    EmployerFinanceConfigurationKeys.ApiClient));
        }
    }
}