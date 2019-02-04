using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Jobs.StartupJobs;
using SFA.DAS.EmployerFinance.Startup;

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
                var environmentName = environmentService.IsCurrent(DasEnv.LOCAL) ? EnvironmentName.Development : EnvironmentName.Production;
                var jobActivator = new StructureMapJobActivator(container);
                var startup = container.GetInstance<IRunAtStartup>();
                
                var host = new HostBuilder()
                    .UseEnvironment(environmentName)
                    .ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers())
                    .ConfigureAppConfiguration(b => b.AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{environmentName}.json", true).AddEnvironmentVariables())
                    .ConfigureLogging(b => b.AddNLog())
                    .ConfigureServices(c => c.AddSingleton<IJobActivator>(jobActivator))
                    .UseConsoleLifetime()
                    .Build();

                var jobHost = host.Services.GetService<IJobHost>();
                
                await startup.StartAsync();
                
                using (host)
                {
                    await jobHost.CallAsync(nameof(DeployDatabaseJob));
                    await host.RunAsync();
                }

                await startup.StopAsync();
            }
        }
    }
}