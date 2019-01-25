using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Startup;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        private const string AppSettingFilePath = "appsettings.json";

        public static async Task Main()
        {
            ServicePointManager.DefaultConnectionLimit = 50;
            
            var environmentVariables = ConfigurationBootstrapper.GetEnvironmentVariables();
            var config = ConfigurationBootstrapper.GetConfiguration(environmentVariables.StorageConnectionString, environmentVariables.EnvironmentName, ConfigurationKeys.EmployerFinance);
            
            using (var container = IoC.Initialize(config, environmentVariables.EnvironmentName))
            {
                var startup = container.GetInstance<IStartup>();

                await startup.StartAsync();

                var jobActivator = new StructureMapJobActivator(container);

                var hostingEnvironment = container.GetInstance<IHostingEnvironment>();

                var hostBuilder = new HostBuilder()
                    .UseEnvironment(hostingEnvironment.EnvironmentName)
                    .ConfigureWebJobs(builder =>
                    {
                        builder.AddAzureStorageCoreServices()
                               .AddTimers();
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
