using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccount
{
    public class AddAccountCommand : IRequest
    {
        internal readonly long Id;
        internal readonly string HashedId;
        internal readonly string PublicHashedId;
        internal readonly string Name;
        internal readonly DateTime Added;

        public AddAccountCommand(long id, string hashedId, string publicHashedId, string name, DateTime added)
        {
            Id = id;
            HashedId = hashedId;
            PublicHashedId = publicHashedId;
            Name = name;
            Added = added;
        }
    }
}