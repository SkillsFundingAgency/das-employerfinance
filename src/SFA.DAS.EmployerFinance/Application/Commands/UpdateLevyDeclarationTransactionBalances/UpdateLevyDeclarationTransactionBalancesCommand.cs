using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationTransactionBalances
{
    public class UpdateLevyDeclarationTransactionBalancesCommand : IRequest
    {
        public int SagaId { get; private set; }

        public UpdateLevyDeclarationTransactionBalancesCommand(int sagaId)
        {
            SagaId = sagaId;
        }
    }
}