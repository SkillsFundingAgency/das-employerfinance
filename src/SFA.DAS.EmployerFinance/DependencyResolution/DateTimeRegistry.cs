using SFA.DAS.EmployerFinance.Services;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class DateTimeRegistry : Registry
    {
        public DateTimeRegistry()
        {
            For<IDateTimeService>().Use<DateTimeService>();
        }
    }
}