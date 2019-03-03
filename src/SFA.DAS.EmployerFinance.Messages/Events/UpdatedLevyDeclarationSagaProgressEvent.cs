namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class UpdatedLevyDeclarationSagaProgressEvent
    {
        public int SagaId { get; }

        public UpdatedLevyDeclarationSagaProgressEvent(int sagaId)
        {
            SagaId = sagaId;
        }
    }
}