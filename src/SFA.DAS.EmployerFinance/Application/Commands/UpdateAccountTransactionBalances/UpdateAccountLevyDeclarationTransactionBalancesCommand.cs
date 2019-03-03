namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances
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