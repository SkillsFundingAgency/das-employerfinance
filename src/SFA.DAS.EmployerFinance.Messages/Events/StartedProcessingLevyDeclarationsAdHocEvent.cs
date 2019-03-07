using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class StartedProcessingLevyDeclarationsAdHocEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }
        public DateTime Started { get; }

        public StartedProcessingLevyDeclarationsAdHocEvent(int sagaId, DateTime payrollPeriod, long accountPayeSchemeId, DateTime started)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
            Started = started;
        }
    }
}