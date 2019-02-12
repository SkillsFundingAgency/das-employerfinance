using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using System.Reflection;
using DbUp;

namespace SFA.DAS.EmployerFinance.Database
{
    public class EmployerFinanceDatabaseHelper
    {
        private readonly ILogger _logger;
        private readonly EmployerFinanceConfiguration _configuration;
       
        public EmployerFinanceDatabaseHelper(ILogger logger, EmployerFinanceConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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