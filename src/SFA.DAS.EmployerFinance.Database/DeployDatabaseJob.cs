using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using System.Reflection;
using DbUp;

namespace SFA.DAS.EmployerFinance.Database
{
    public class DeployDatabaseJob
    {
        private readonly ILogger _logger;
        private readonly EmployerFinanceConfiguration _configuration;
       
        public DeployDatabaseJob(EmployerFinanceConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public void Deploy(string connectionString = null)
        {
            _logger.LogInformation("Started deploying database");

            var databaseConnectionString = connectionString ?? _configuration.DatabaseConnectionString;
            
            EnsureDatabase.For.SqlDatabase(databaseConnectionString);
        
            var upgradeEngine = DeployChanges.To
                .SqlDatabase(databaseConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(EmployerFinanceConfiguration)))
                .WithTransaction()
                .LogToAutodetectedLog()
                .Build();

            var result = upgradeEngine.PerformUpgrade();

            if (result.Successful)
            {
                _logger.LogInformation("Finished deploying database");
            }
            else
            {
                throw result.Error;
            }
        }
    }
}