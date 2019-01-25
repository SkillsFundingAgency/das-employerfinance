using System.Reflection;
using DbUp;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;

namespace SFA.DAS.EmployerFinance.Jobs.StartupJobs
{
    public class DeployDatabaseJob
    {
        private readonly EmployerFinanceConfiguration _configuration;

        public DeployDatabaseJob(EmployerFinanceConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void Run([TimerTrigger("0 0 * * *", RunOnStartup = true)]TimerInfo timer, ILogger logger)
        {
            logger.LogInformation("Started deploying database");
            
            EnsureDatabase.For.SqlDatabase(_configuration.DatabaseConnectionString);
            
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(_configuration.DatabaseConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(EmployerFinanceConfiguration)))
                .WithTransaction()
                .LogToAutodetectedLog()
                .Build();

            var result = upgradeEngine.PerformUpgrade();

            if (result.Successful)
            {
                logger.LogInformation("Finished deploying database");
            }
            else
            {
                logger.LogError(result.Error.Message);
            }
        }
    }
}