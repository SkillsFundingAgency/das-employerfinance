using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class StartedProcessingLevyDeclarationsEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public DateTime Started { get; }

        public StartedProcessingLevyDeclarationsEvent(int sagaId, DateTime payrollPeriod, DateTime started)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            Started = started;
        }
    }
}