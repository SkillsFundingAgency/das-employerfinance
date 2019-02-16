using Microsoft.Extensions.Logging;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            For<ILogger>().Use(c => c.GetInstance<ILoggerFactory>().CreateLogger(c.ParentType));
        }
    }
}