using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck.Dtos;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck
{
    public class GetHealthCheckQueryResult
    {
        public HealthCheckDto HealthCheck { get; }

        public GetHealthCheckQueryResult(HealthCheckDto healthCheck)
        {
            HealthCheck = healthCheck;
        }
    }
}