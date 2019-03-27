using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccount
{
    public class AddAccountCommand : IRequest
    {
        public long Id { get; }
        public string HashedId { get; }
        public string PublicHashedId { get; }
        public string Name { get; }
        public DateTime Added { get; }

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