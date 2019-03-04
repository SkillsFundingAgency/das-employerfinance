using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using NLog.Web;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class LoggingStartup
    {
        public static IHostBuilder ConfigureDasLogging(this IHostBuilder builder)
        {
            return builder.ConfigureLogging(b => b.AddNLog());
        }
        
        public static IWebHostBuilder ConfigureDasLogging(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseNLog();
        }
    }
}