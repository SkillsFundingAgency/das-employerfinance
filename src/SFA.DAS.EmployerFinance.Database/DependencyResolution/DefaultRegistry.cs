using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Types.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Database.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IConfiguration>().Use(() => new ConfigurationBuilder().AddAzureTableStorage(EmployerFinanceConfigurationKeys.Base).Build()).Singleton();
            For<ILoggerFactory>().Use(() => new LoggerFactory().AddNLog()).Singleton();
        }
    }
}