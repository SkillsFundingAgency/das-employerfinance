using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;

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

                if (environmentService.IsCurrent(DasEnv.LOCAL))
                {
                    config.UseDevelopmentSettings();
                }

                config.UseTimers();

                var jobHost = new JobHost(config);
                
                jobHost.RunAndBlock();
            }
        }
    }
}
