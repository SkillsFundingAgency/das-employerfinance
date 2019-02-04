using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using IStartup = SFA.DAS.EmployerFinance.Startup.IStartup;

namespace SFA.DAS.EmployerFinance.MessageHandlers
{
    public static class Program
    {
        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            var environmentVariables = ConfigurationBootstrapper.GetEnvironmentVariables();
            var config = ConfigurationBootstrapper.GetConfiguration(environmentVariables.StorageConnectionString, environmentVariables.EnvironmentName, ConfigurationKeys.EmployerFinance);

            using (var container = IoC.Initialize(config, environmentVariables.EnvironmentName))
            {
                var startup = container.GetInstance<IRunAtStartup>();
                var hostingEnvironment = container.GetInstance<IHostingEnvironment>();
                var jobActivator = new StructureMapJobActivator(container);
            
                var host = new HostBuilder()
                    .UseEnvironment(hostingEnvironment.EnvironmentName)
                    .ConfigureWebJobs(b => b.AddAzureStorageCoreServices().AddTimers())
                    .ConfigureAppConfiguration(b => b.AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true).AddEnvironmentVariables())
                    .ConfigureLogging(b => b.AddNLog())
                    .ConfigureServices(c => c.AddSingleton<IJobActivator>(jobActivator))
                    .UseConsoleLifetime()
                    .Build();
            
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
