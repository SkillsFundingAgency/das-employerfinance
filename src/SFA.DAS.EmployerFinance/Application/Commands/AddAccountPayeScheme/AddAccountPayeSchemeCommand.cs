using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme
{
    public class AddAccountPayeSchemeCommand : IRequest
    {
        internal readonly long AccountId;
        internal readonly string EmployerReferenceNumber;
        internal readonly DateTime Created;

        public AddAccountPayeSchemeCommand(long accountId, string employerReferenceNumber, DateTime created)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Created = created;
        }
    }
}