using System;
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
        
        [FunctionName(nameof(DeployDatabaseJob))]
        [NoAutomaticTrigger]
        public void Run(ILogger logger)
        {
            logger.LogInformation("Started deploying database");
            
            EnsureDatabase.For.SqlDatabase(_configuration.DatabaseConnectionString);
            
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(_configuration.DatabaseConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(EmployerFinanceConfiguration)))
                .WithTransaction()
                .WithExecutionTimeout(TimeSpan.FromSeconds(180))
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