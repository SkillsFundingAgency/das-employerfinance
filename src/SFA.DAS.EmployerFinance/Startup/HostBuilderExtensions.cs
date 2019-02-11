using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureDasAppConfiguration(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((c, b) => b
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{c.HostingEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables()
                .AddAzureTableStorage(EmployerFinanceConfigurationKeys.Base));
        }

        public static IHostBuilder ConfigureDasLogging(this IHostBuilder builder)
        {
            return builder.ConfigureLogging(b => b.AddNLog());
        }

        public static IHostBuilder ConfigureDasWebJobs(this IHostBuilder builder)
        {
            builder.ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers());
            
#pragma warning disable 618
            builder.ConfigureServices(s => s.AddSingleton<IWebHookProvider>(p => null));
#pragma warning restore 618

            return builder;
        }
        
        public static IHostBuilder UseStructureMap(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new StructureMapServiceProviderFactory(null));
        }
    }
}