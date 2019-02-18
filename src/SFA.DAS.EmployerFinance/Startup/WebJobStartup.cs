using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class WebJobStartup
    {
        public static IHostBuilder ConfigureDasWebJobs(this IHostBuilder builder)
        {
            builder.ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers());
            
#pragma warning disable 618
            builder.ConfigureServices(s => s.AddSingleton<IWebHookProvider>(p => null));
#pragma warning restore 618

            return builder;
        }
    }
}