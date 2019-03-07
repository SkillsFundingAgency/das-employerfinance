using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class FinishedProcessingLevyDeclarationsAdHocEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }
        public DateTime Finished { get; }

        public FinishedProcessingLevyDeclarationsAdHocEvent(int sagaId, DateTime payrollPeriod, long accountPayeSchemeId, DateTime finished)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
            Finished = finished;
        }
    }
}