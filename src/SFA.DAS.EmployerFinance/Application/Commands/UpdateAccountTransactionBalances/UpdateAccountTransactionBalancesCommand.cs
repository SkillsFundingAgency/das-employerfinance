using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances
{
    public abstract class UpdateAccountTransactionBalancesCommand : IRequest
    {
        public long AccountId { get; }

        protected UpdateAccountTransactionBalancesCommand(long accountId)
        {
            AccountId = accountId;
        }
    }
}