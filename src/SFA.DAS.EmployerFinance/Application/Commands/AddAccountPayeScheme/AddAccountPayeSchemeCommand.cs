using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme
{
    public class AddAccountPayeSchemeCommand : IRequest
    {
        public long AccountId { get; }
        public string EmployerReferenceNumber { get; }
        public DateTime Added { get; }

        public AddAccountPayeSchemeCommand(long accountId, string employerReferenceNumber, DateTime added)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Added = added;
        }
    }
}