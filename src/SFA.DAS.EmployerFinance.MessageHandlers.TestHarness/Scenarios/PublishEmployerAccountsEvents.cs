using System;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.EmployerAccounts.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Scenarios
{
    public class PublishEmployerAccountsEvents
    {
        private readonly IMessageSession _messageSession;

        public PublishEmployerAccountsEvents(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run()
        {
            const long accountId = 80085L;
            const string firstAccountName = "first";
            const string secondAccountName = "second";
            const string employerReferenceNumber = "ABC/AB56789";
            
            await _messageSession.Publish(new CreatedAccountEvent
            {
                AccountId = accountId,
                Name = firstAccountName,
                HashedId = "pvHash",
                PublicHashedId = "pbHash",
                Created = DateTime.UtcNow
            });

            await _messageSession.Publish(new ChangedAccountNameEvent
            {
                AccountId = accountId,
                CurrentName = secondAccountName,
                Created = DateTime.UtcNow
            });

            await _messageSession.Publish(new AddedPayeSchemeEvent
            {
                AccountId = accountId,
                PayeRef = employerReferenceNumber,
                Created = DateTime.UtcNow
            });

            await _messageSession.Publish(new DeletedPayeSchemeEvent
            {
                AccountId = accountId,
                PayeRef = employerReferenceNumber,
                Created = DateTime.UtcNow
            });
        }
    }
}