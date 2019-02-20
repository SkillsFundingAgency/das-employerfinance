using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class AccountPayeScheme
    {
        public long Id { get; private set; }
        public Account Account { get; private set; }
        public long AccountId { get; private set; }
        public PayeScheme PayeScheme { get; private set; }
        public string EmployerReferenceNumber { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public DateTime? Deleted { get; private set; }
        
        internal AccountPayeScheme(long accountId, string employerReferenceNumber, DateTime created)
        {
            AccountId = accountId;
            EmployerReferenceNumber = employerReferenceNumber;
            Created = created;
        }

        private AccountPayeScheme()
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