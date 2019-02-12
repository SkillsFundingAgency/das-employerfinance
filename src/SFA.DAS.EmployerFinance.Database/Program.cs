using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinance.Database.DependencyResolution;
using SFA.DAS.EmployerFinance.Extensions;

namespace SFA.DAS.EmployerFinance.Database
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            using (var container = IoC.Initialize())
            {
                var loggerFactory = container.GetInstance<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<Program>();
                var helper = container.GetInstance<EmployerFinanceDatabaseHelper>();
                
                try
                {
                    helper.Deploy(args?.FirstOrDefault());
                }
                catch (Exception e)
                {
                    logger.LogError(e.GetAggregateMessage(), e);
                    
                    return -1;
                }
            }

            return 0;
        }
    }
}