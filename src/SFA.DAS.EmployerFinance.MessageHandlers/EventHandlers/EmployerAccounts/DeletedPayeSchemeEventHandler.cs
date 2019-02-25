using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class DeletedPayeSchemeEventHandler : IHandleMessages<DeletedPayeSchemeEvent>
    {
        public DeletedPayeSchemeEventHandler()
        {
        }

        public Task Handle(DeletedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}