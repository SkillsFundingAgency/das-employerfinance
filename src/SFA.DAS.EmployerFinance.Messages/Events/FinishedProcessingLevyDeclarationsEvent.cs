using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class FinishedProcessingLevyDeclarationsEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeHighWaterMarkId { get; }
        public DateTime Finished { get; }

        public FinishedProcessingLevyDeclarationsEvent(int sagaId, DateTime payrollPeriod, long accountPayeSchemeHighWaterMarkId, DateTime finished)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeHighWaterMarkId = accountPayeSchemeHighWaterMarkId;
            Finished = finished;
        }
    }
}