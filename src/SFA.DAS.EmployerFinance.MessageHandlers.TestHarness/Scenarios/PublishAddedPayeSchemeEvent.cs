using System.Threading.Tasks;
using NServiceBus;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Scenarios
{
    public class PublishAddedPayeSchemeEvent
    {
        private readonly IMessageSession _messageSession;

        public PublishAddedPayeSchemeEvent(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Run()
        {
            return Task.CompletedTask;
        }
    }
}