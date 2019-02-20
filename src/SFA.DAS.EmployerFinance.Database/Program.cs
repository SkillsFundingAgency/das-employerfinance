using System;
using System.Linq;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Database.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Database
{
    public static class Program
    {
        private static int Main(string[] args)
        {
            using (var container = IoC.Initialize())
            {
                var loggerFactory = container.GetInstance<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger(typeof(Program));
                var employerFinanceConfiguration = container.GetInstance<EmployerFinanceConfiguration>();
                var databaseConnectionString = args?.FirstOrDefault() ?? employerFinanceConfiguration.DatabaseConnectionString;
                
                logger.LogInformation("Started deploying database");
            
                EnsureDatabase.For.SqlDatabase(databaseConnectionString, 120);
        
                var upgradeEngine = DeployChanges.To
                    .SqlDatabase(databaseConnectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetAssembly(typeof(EmployerFinanceConfiguration)))
                    .WithTransaction()
                    .LogToAutodetectedLog()
                    .WithExecutionTimeout(TimeSpan.FromMinutes(10))
                    .Build();

                var result = upgradeEngine.PerformUpgrade();

                if (!result.Successful)
                {
                    logger.LogError(result.Error, "Failed deploying database");

                    return -1;
                }
                
                logger.LogInformation("Finished deploying database");

                return 0;
            }
        }
    }
}