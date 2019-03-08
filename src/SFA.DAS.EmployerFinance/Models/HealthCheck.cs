using System;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.Models
{
    public class HealthCheck : Entity
    {
        public virtual int Id { get; private set; }
        public virtual DateTime SentEmployerFinanceApiRequest { get; private set; }
        public virtual DateTime? ReceivedEmployerFinanceApiResponse { get; private set; }
        public virtual DateTime PublishedEmployerFinanceEvent { get; private set; }
        public virtual DateTime? ReceivedEmployerFinanceEvent { get; private set; }
        
        public virtual async Task Run(Func<Task> employerFinanceApiRequest)
        {
            await SendEmployerFinanceApiRequest(employerFinanceApiRequest);
            PublishEmployerFinanceEvent();
        }

        public virtual void ReceiveEmployerFinanceEvent()
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