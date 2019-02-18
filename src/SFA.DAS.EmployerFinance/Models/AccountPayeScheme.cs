using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class AccountPayeScheme : Entity
    {
        public long Id { get; private set; }
        public Account Account { get; private set; }
        public long AccountId { get; private set; }
        public string EmployerReferenceNumber { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Deleted { get; private set; }

        public AccountPayeScheme(Account account, string employerReferenceNumber)
        {
            Account = account;
            AccountId = account.Id;
            EmployerReferenceNumber = employerReferenceNumber;
        }

        private AccountPayeScheme()
        {
        }
    }
}