using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme
{
    public class AddAccountPayeSchemeCommand : IRequest
    {
        internal readonly long AccountId;
        internal readonly string EmployerReferenceNumber;
        internal readonly DateTime Added;

        public AddAccountPayeSchemeCommand(long accountId, string employerReferenceNumber, DateTime added)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Added = added;
        }
    }
}