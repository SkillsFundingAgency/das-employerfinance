using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages;

namespace SFA.DAS.EmployerFinance.Jobs.ScheduledJobs
{
    public class DummyJob
    {
        private readonly IMessageSession _messageSession;

        public DummyJob(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Run([TimerTrigger("*/2 * * * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            logger.LogDebug("Running Dummy Job...");
            
            Console.WriteLine("Publishing a new dummy event...");

            await _messageSession.SendLocal(new DummyEvent{Payload = $"Dummy Event Sent [{DateTime.Now:hh:mm:ss}]"});
        }
    }
}
