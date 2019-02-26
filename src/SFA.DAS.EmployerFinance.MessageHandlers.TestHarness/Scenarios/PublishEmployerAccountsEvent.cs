using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Scenarios
{
    public class PublishEmployerAccountsEvent
    {
        private readonly IMessageSession _messageSession;

        public PublishEmployerAccountsEvent(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            const long accountId = 80085L;
            const string firstAccountName = "first";
            //const string secondAccountName = "second";
            
            await _messageSession.Publish(new CreatedAccountEvent
            {
                AccountId = accountId,
                Name = firstAccountName,
                HashedId = "hashedId",
                PublicHashedId = "pbHash",
                Created = DateTime.UtcNow
            });
        }
    }
}