using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances
{
    public class UpdateAccountLevyDeclarationTransactionBalancesCommand : UpdateAccountTransactionBalancesCommand
    {
        public int SagaId { get; }

        public UpdateAccountLevyDeclarationTransactionBalancesCommand(int sagaId, long accountId) : base(accountId)
        {
            SagaId = sagaId;
        }
    }
}