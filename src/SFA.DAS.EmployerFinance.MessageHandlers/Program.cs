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
        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            using (var container = IoC.Initialize())
            {
                var startup = container.GetInstance<IStartup>();
                var environmentService = container.GetInstance<IEnvironmentService>();
                var environmentName = environmentService.IsCurrent(DasEnv.LOCAL) ? EnvironmentName.Development : EnvironmentName.Production;
                var jobActivator = new StructureMapJobActivator(container);
            
                var host = new HostBuilder()
                    .UseEnvironment(environmentName)
                    .ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers())
                    .ConfigureAppConfiguration(b => b.AddJsonFile(environmentName).AddEnvironmentVariables())
                    .ConfigureLogging(b => b.AddNLog())
                    .ConfigureServices(c => c.AddSingleton<IJobActivator>(jobActivator))
                    .UseConsoleLifetime()
                    .Build();
                
                var loggerProvider = host.Services.GetService<ILoggerProvider>();
                container.Configure(c => c.For<ILogger>().Use(x => loggerProvider.CreateLogger(x.ParentType.FullName)));
            
                await startup.StartAsync();

                using (host)
                {
                    await host.RunAsync();
                }

                await startup.StopAsync();
            }
        }
    }
}
