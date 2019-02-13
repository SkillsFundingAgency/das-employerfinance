using NServiceBus;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.MessageHandlers.HealthChecks;
using SFA.DAS.EmployerFinance.Messages.Messages;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IEmployerFinanceDbContextFactory>().Use<EmployerFinanceDbContextWithNServiceBusTransactionFactory>();
            For<IHandleMessages<HealthCheckRequestMessage>>().Use<HealthCheckRequestMessageHandler>();
        }
    }
}