using System;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Startup
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

        public static IWebHostBuilder UseDasEnvironment(this IWebHostBuilder hostBuilder)
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentVariableNames.EnvironmentName);
            var mappedEnvironmentName = DasEnvironmentName.Map[environmentName];
            
            return hostBuilder.UseEnvironment(mappedEnvironmentName);
        }
    }
}