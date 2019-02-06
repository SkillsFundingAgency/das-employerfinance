using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using System.Reflection;
using DbUp;

namespace SFA.DAS.EmployerFinance.Database.Console
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

        public void Deploy()
        {
            _logger.LogInformation("Started deploying database");
        
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
                _logger.LogInformation("Finished deploying database");
            }
            else
            {
                throw result.Error;
            }
        }
    }
}