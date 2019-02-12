using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Database.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<ILoggerFactory>().Use<NLogLoggerFactory>();
            For<EmployerFinanceConfiguration>().Use(c => c.GetInstance<IAutoConfigurationService>().Get<EmployerFinanceConfiguration>(ConfigurationKeys.EmployerFinance)).Singleton();
            For<EmployerFinanceDatabaseHelper>().Use<EmployerFinanceDatabaseHelper>();        
        }
    }
}