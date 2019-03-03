using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class FinishedProcessingLevyDeclarationsEvent
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public DateTime Finished { get; }

        public FinishedProcessingLevyDeclarationsEvent(int sagaId, DateTime payrollPeriod, DateTime finished)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            Finished = finished;
        }
    }
}