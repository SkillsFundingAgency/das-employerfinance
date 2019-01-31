using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.Models
{
    public class HealthCheck : Entity
    {
        public int Id { get; private set; }
        public DateTime SentEmployerFinanceApiRequest { get; private set; }
        public DateTime? ReceivedEmployerFinanceApiResponse { get; private set; }
        public DateTime PublishedEmployerFinanceEvent { get; private set; }
        public DateTime? ReceivedEmployerFinanceEvent { get; private set; }
        
        public async Task Run(Func<Task> employerFinanceApiRequest)
        {
            await SendEmployerFinanceApiRequest(employerFinanceApiRequest);
            PublishEmployerFinanceEvent();
        }

        public void ReceiveEmployerFinanceEvent()
        {
            ReceivedEmployerFinanceEvent = DateTime.UtcNow;
        }

        private async Task SendEmployerFinanceApiRequest(Func<Task> run)
        {
            SentEmployerFinanceApiRequest = DateTime.UtcNow;

            try
            {
                await run();
                ReceivedEmployerFinanceApiResponse = DateTime.UtcNow;
            }
            catch (Exception)
            {
            }
        }

        private void PublishEmployerFinanceEvent()
        {
            PublishedEmployerFinanceEvent = DateTime.UtcNow;

            Publish(() => new HealthCheckEvent(Id, PublishedEmployerFinanceEvent));
        }
    }
}