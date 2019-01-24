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
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers
{
    public static class Program
    {
        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            using (var container = IoC.Initialize())
            {
                var environmentSettings = GetEnvironmentSettings(container);

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

        private static IHost CreateHost(IContainer container, EnvironmentSettings environmentSettings)
        {
            var jobActivator = new StructureMapJobActivator(container);

            var hostBuilder = new HostBuilder()
                .UseEnvironment(environmentSettings.EnvironmentName)
                .ConfigureWebJobs(builder => { builder.AddAzureStorageCoreServices(); })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile(environmentSettings.AppSettingsFilePath);
                    builder.AddEnvironmentVariables();
                })
                .ConfigureLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Debug)
                        .AddNLog();
                })
                .ConfigureServices(collection => { collection.AddSingleton<IJobActivator>(jobActivator); })
                .UseConsoleLifetime();

            var host = hostBuilder.Build();

            AddHostLoggerToIoC(container, host);

            return host;
        }

        private static void AddHostLoggerToIoC(IContainer container, IHost host)
        {
            var loggerProvider = host.Services.GetService<ILoggerProvider>();
            container.Configure(c => c.For<ILogger>().Use(x => loggerProvider.CreateLogger(x.ParentType.FullName)));
        }

        private static EnvironmentSettings GetEnvironmentSettings(IContainer container)
        {
            var settings = new EnvironmentSettings();
            
            var environmentService = container.GetInstance<IEnvironmentService>();

            settings.EnvironmentName = EnvironmentName.Production;
            settings.AppSettingsFilePath = "appsettings.json";

            if (!environmentService.IsCurrent(DasEnv.LOCAL))
            {
                return settings;
            }
            
            settings.EnvironmentName = EnvironmentName.Development;
            settings.AppSettingsFilePath = "appsettings.Development.json";

            return settings;
        }

        private struct EnvironmentSettings
        {
            public string EnvironmentName { get; set; }
            public string AppSettingsFilePath { get; set; }
        }
    }
}
