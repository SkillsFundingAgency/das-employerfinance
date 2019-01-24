using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            using (var container = IoC.Initialize())
            {
                var environmentService = container.GetInstance<IEnvironmentService>();

                var environmentSettings = HostEnvironmentSettingsFactory.Create(environmentService);

                var host = CreateHost(container, environmentSettings);

                await RunHost(container, host);
            }
        }
        
          private static async Task RunHost(IContainer container, IHost host)
        {
            var startup = container.GetInstance<IStartup>();

            await startup.StartAsync();

            using (host)
            {
                await host.RunAsync();
            }

            await startup.StopAsync();
        }

        private static IHost CreateHost(IContainer container, HostEnvironmentSettings environmentSettings)
        {
            var jobActivator = new StructureMapJobActivator(container);
            
            var hostBuilder = new HostBuilder()
                .UseEnvironment(environmentSettings.EnvironmentName)
                .ConfigureWebJobs(builder =>
                {
                    builder.AddAzureStorageCoreServices()
                           .AddTimers();;
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile(environmentSettings.AppSettingsFilePath);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureLogging(builder =>
                {
                    builder.SetMinimumLevel(environmentSettings.MinLogLevel)
                        .AddNLog();
                })
                .ConfigureServices(collection => { collection.AddSingleton<IJobActivator>(jobActivator); })
                .UseConsoleLifetime();
            
            var host = hostBuilder.Build();
            
            return host;
        }
    }
}
