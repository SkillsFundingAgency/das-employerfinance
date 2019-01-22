using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution;
using SFA.DAS.EmployerFinance.Startup;

namespace SFA.DAS.EmployerFinance.MessageHandlers
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

                var environmentService = container.GetInstance<IEnvironmentService>();

                var environmentName = environmentService.IsCurrent(DasEnv.LOCAL)
                    ? EnvironmentName.Development
                    : EnvironmentName.Production;

                var hostBuilder = new HostBuilder()
                    .UseEnvironment(environmentName)
                    .ConfigureWebJobs(builder =>
                    {
                        builder.AddAzureStorageCoreServices();
                    })
                    .ConfigureAppConfiguration(builder =>
                    {
                        builder.AddJsonFile(AppSettingFilePath);
                    })
                    .ConfigureLogging(builder => 
                    { 
                        builder.SetMinimumLevel(LogLevel.Debug)
                               .AddNLog();
                    })
                    .ConfigureServices(collection =>
                    {
                        collection.AddSingleton<IJobActivator>(jobActivator);
                    })
                    .UseConsoleLifetime();

                var host = hostBuilder.Build();

                using (host)
                {
                    await host.RunAsync();
                }

                await startup.StopAsync();
            }
        }
    }
}
