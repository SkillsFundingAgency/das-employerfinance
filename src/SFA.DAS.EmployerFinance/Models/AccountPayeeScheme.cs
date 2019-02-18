using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class AccountPayeeScheme
    {
        public Account Account { get; private set; }
        public long AccountId { get; private set; }
        public PayeScheme PayeeScheme { get; private set; }
        public string EmployerReferenceNumber { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public DateTime? Deleted { get; private set; }
        
        internal AccountPayeeScheme(long accountId, string employerReferenceNumber, DateTime created)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Created = created;
        }

        private AccountPayeeScheme()
        {
        }

        internal void Delete(DateTime deleted)
        {
            EnsureHasNotBeenDeleted();
            
            Deleted = deleted;
        }

        private void EnsureHasNotBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires account payee scheme has not been deleted");
            }
        }
    }
}