using System;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck.Dtos
{
    public class HealthCheckDto
    {
        public int Id { get; set; }
        public DateTime SentEmployerFinanceApiRequest { get; set; }
        public DateTime? ReceivedEmployerFinanceApiResponse { get; set; }
        public DateTime PublishedEmployerFinanceEvent { get; set; }
        public DateTime? ReceivedEmployerFinanceEvent { get; set; }
    }
}