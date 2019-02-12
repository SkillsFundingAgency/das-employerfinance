using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Database.DependencyResolution;
using SFA.DAS.EmployerFinance.Database.Jobs;
using SFA.DAS.EmployerFinance.Extensions;

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
                var deployDatabaseJob = container.GetInstance<DeployDatabaseJob>();
                
                try
                {
                    deployDatabaseJob.Deploy(args?.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.GetAggregateMessage(), ex);
                    
                    return -1;
                }
            }

            return 0;
        }
    }
}