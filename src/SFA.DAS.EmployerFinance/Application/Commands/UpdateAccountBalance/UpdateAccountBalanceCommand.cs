using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance
{
    public class UpdateAccountBalanceCommand : IRequest
    {
        public Guid JobId { get; }
        public long AccountId { get; }

        public UpdateAccountBalanceCommand(Guid jobId, long accountId)
        {
            JobId = jobId;
            AccountId = accountId;
        }
    }
}