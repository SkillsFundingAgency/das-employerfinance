using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Jobs.StartupJobs;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.EmployerFinance.Types.Configuration;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
            
            var environmentVariables = ConfigurationBootstrapper.GetEnvironmentVariables();
            var configurationRoot = ConfigurationBootstrapper.GetConfiguration(environmentVariables.StorageConnectionString, environmentVariables.EnvironmentName, EmployerFinanceConfigurationKeys.Base);
            
            using (var container = IoC.Initialize(environmentVariables.EnvironmentName, configurationRoot))
            {
                var jobActivator = new StructureMapJobActivator(container);
                var hostingEnvironment = container.GetInstance<IHostingEnvironment>();

                var startup = container.GetInstance<IRunAtStartup>();
                
                var host = new HostBuilder()
                    .UseEnvironment(hostingEnvironment.EnvironmentName)
                    .ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers())
                    .ConfigureAppConfiguration(b => b.AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true).AddEnvironmentVariables())
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