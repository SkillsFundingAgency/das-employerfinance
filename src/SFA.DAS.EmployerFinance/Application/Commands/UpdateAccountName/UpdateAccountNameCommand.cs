using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountName
{
    public class UpdateAccountNameCommand : IRequest
    {
        public long AccountId { get; }
        public string Name { get; }
        public DateTime Updated { get; }

        public UpdateAccountNameCommand(long accountId, string name, DateTime updated)
        {
            AccountId = accountId;
            Name = name;
            Updated = updated;
        }
    }
}