using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccount
{
    public class UpdateAccountCommand : IRequest
    {
        internal readonly long AccountId;
        internal readonly string Name;
        internal readonly DateTime Updated;

        public UpdateAccountCommand(long accountId, string name, DateTime updated)
        {
            AccountId = accountId;
            Name = name;
            Updated = updated;
        }
    }
}