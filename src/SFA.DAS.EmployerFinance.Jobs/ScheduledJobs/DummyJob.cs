using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using NServiceBus;


namespace SFA.DAS.EmployerFinance.Jobs.ScheduledJobs
{
    public class DummyJob
    {
        private readonly IMessageSession _messageSession;

        public DummyJob(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] TimerInfo timer, ILogger logger)
        {
            logger.LogDebug("Running Dummy Job...");

            //INSERT CODE HERE TO RUN

            return Task.CompletedTask;
        }
    }
}
