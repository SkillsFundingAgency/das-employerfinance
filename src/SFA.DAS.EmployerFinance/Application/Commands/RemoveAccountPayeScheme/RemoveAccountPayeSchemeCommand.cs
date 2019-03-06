using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme
{
    public class RemoveAccountPayeSchemeCommand : IRequest
    {
        public long AccountId { get; }
        public string EmployerReferenceNumber { get; }
        public DateTime Removed { get; }

        public RemoveAccountPayeSchemeCommand(long accountId, string employerReferenceNumber, DateTime removed)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Removed = removed;
        }
    }
}