using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class CreatedAccountEventHandler : IHandleMessages<CreatedAccountEvent>
    {
        public CreatedAccountEventHandler()
        {
        }

        public Task Handle(CreatedAccountEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}