using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Scenarios;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<PublishEmployerAccountsEvents>().Use<PublishEmployerAccountsEvents>();
        }
    }
}