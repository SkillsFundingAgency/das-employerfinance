using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress
{
    public class UpdateLevyDeclarationSagaProgressCommand : IRequest
    {
        public int SagaId { get; }

        public UpdateLevyDeclarationSagaProgressCommand(int sagaId)
        {
            SagaId = sagaId;
        }
    }
}