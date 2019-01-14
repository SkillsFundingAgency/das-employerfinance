using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Startup;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        private const string AppSettingFilePath = "appsettings.json";

        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            using (var container = IoC.Initialize())
            {
                var startup = container.GetInstance<IStartup>();

                await startup.StartAsync();

                var jobActivator = new StructureMapJobActivator(container);

                var builder = new HostBuilder()
                    .UseEnvironment("Development")
                    .ConfigureWebJobs(b =>
                    {
                        b.AddAzureStorageCoreServices()
                         .AddTimers();
                    })
                    .ConfigureAppConfiguration(configurationBuilder =>
                    {
                        configurationBuilder.AddJsonFile(AppSettingFilePath);
                    })
                    .ConfigureLogging((context, loggerBuilder) => 
                    { 
                        loggerBuilder.SetMinimumLevel(LogLevel.Debug);
                        loggerBuilder.AddNLog();
                    })
                    .ConfigureServices((context, collection) =>
                    {
                        //Wire up the IOC container to the built in web job IOC container
                        collection.AddSingleton<IJobActivator>(jobActivator);
                    })
                    .UseConsoleLifetime();

                var host = builder.Build();

                using (host)
                {
                    await host.RunAsync();
                }

                await startup.StopAsync();
            }
        }
    }
}
