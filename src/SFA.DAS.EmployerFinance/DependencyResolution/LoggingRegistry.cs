using Microsoft.Extensions.Logging;
using StructureMap;


namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class LoggingRegistry : Registry
    {
        public LoggingRegistry()
        {
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}