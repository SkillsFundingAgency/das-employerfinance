using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts
{
    public class AddedPayeSchemeEventHandler : IHandleMessages<AddedPayeSchemeEvent>
    {
        public AddedPayeSchemeEventHandler()
        {
        }

        public Task Handle(AddedPayeSchemeEvent message, IMessageHandlerContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}