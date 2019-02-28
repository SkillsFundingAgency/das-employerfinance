using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Jobs.ScheduledJobs;

namespace SFA.DAS.EmployerFinance.Jobs.Startup
{
    public static class WebJobStartup
    {
        public static IHostBuilder ConfigureDasWebJobs(this IHostBuilder builder)
        {
            builder.ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers());
            
#pragma warning disable 618
            builder.ConfigureServices(s => s.AddSingleton<IWebHookProvider>(p => null));
#pragma warning restore 618

            builder.ConfigureServices(s => s.AddSingleton<ImportProvidersJob>());

            return builder;
        }
    }
}