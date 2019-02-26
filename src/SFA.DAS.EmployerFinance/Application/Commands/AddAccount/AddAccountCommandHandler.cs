using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Application.Commands.AddAccount
{
    public class AddAccountCommandHandler : RequestHandler<AddAccountCommand>
    {
        private readonly Lazy<EmployerFinanceDbContext> _db;

        public AddAccountCommandHandler(Lazy<EmployerFinanceDbContext> db)
        {
            _db = db;
        }

        protected override void Handle(AddAccountCommand request)
        {
            var account = new Account(request.Id, request.HashedId, request.PublicHashedId, request.Name, request.Added);
            
            _db.Value.Accounts.Add(account);
        }
    }
}