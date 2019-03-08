using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Models
{
    public class Account : Entity
    {
        public long Id { get; private set; }
        public string HashedId { get; private set; }
        public string PublicHashedId { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public IEnumerable<AccountPayeScheme> AccountPayeSchemes => _accountPayeSchemes;

        private readonly List<AccountPayeScheme> _accountPayeSchemes = new List<AccountPayeScheme>();

        public Account(long id)
        {
            Id = id;
        }

        private Account()
        {
        }
    }
}