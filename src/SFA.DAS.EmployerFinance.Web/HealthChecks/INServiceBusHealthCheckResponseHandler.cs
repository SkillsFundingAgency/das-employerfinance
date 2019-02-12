using System;

namespace SFA.DAS.EmployerFinance.Web.HealthChecks
{
    public interface INServiceBusHealthCheckResponseHandler
    {
        event EventHandler<Guid> ReceivedResponse;
    }
}