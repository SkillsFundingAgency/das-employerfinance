using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Startup;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static async Task Main()
        {
            using (var container = IoC.Initialize())
            {
                
                var config = new JobHostConfiguration { JobActivator = new StructureMapJobActivator(container) };
                var environmentService = container.GetInstance<IEnvironmentService>();
                var loggerFactory = container.GetInstance<ILoggerFactory>();
                var startup = container.GetInstance<IStartup>();

                if (environmentService.IsCurrent(DasEnv.LOCAL))
                {
                    config.UseDevelopmentSettings();
                }

                config.LoggerFactory = loggerFactory;
                config.UseTimers();

                await startup.StartAsync();

                var jobHost = new JobHost(config);
                
                jobHost.RunAndBlock();

                await startup.StopAsync();
            }
        }
    }
}
