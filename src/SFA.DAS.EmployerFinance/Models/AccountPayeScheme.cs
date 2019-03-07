using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class AccountPayeScheme : Entity
    {
        public long Id { get; internal set; }
        public Account Account { get; internal set; }
        public long AccountId { get; internal set; }
        public string EmployerReferenceNumber { get; internal set; }
        public DateTime Created { get; internal set; }
        public DateTime? Deleted { get; internal set; }
        
        internal AccountPayeScheme(Account account, string employerReferenceNumber, DateTime created)
        {
            Account = account;
            AccountId = account.Id;
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

        //we could make deletion idempotent, so if a soft deleted entity is deleted, it's a no-op. or is it better to explicitly check and throw??
        private void EnsureHasNotBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires account payee scheme has not been deleted");
            }
        }
    }
}