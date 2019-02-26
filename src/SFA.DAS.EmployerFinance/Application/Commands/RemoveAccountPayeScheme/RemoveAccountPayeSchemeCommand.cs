using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme
{
    public class RemoveAccountPayeSchemeCommand : IRequest
    {
        internal readonly long AccountId;
        internal readonly string EmployerReferenceNumber;
        internal readonly DateTime Removed;

        public RemoveAccountPayeSchemeCommand(long accountId, string employerReferenceNumber, DateTime removed)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Removed = removed;
        }
    }
}