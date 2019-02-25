using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class ChangedAccountNameEventHandler : IHandleMessages<ChangedAccountNameEvent>
    {
        public ChangedAccountNameEventHandler()
        {
        }

        public Task Handle(ChangedAccountNameEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}