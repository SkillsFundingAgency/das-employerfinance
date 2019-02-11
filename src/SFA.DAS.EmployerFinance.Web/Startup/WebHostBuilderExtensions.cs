using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder ConfigureDasAppConfiguration(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureAppConfiguration(c => c.AddAzureTableStorage(
                EmployerFinanceConfigurationKeys.Base,
                EmployerFinanceConfigurationKeys.ApiClient));
        }

        public static IWebHostBuilder ConfigureDasLogging(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseNLog();
        }
    }
}