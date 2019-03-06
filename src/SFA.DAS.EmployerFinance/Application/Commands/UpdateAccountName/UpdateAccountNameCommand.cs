using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountName
{
    public class UpdateAccountNameCommand : IRequest
    {
        internal readonly long AccountId;
        internal readonly string Name;
        internal readonly DateTime Updated;

        public UpdateAccountNameCommand(long accountId, string name, DateTime updated)
        {
            AccountId = accountId;
            Name = name;
            Updated = updated;
        }
    }
}