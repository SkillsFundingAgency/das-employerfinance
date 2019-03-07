using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class StartedProcessingLevyDeclarationsEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeHighWaterMarkId { get; }
        public DateTime Started { get; }

        public StartedProcessingLevyDeclarationsEvent(int sagaId, DateTime payrollPeriod, long accountPayeSchemeHighWaterMarkId, DateTime started)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeHighWaterMarkId = accountPayeSchemeHighWaterMarkId;
            Started = started;
        }
    }
}