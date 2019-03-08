using System;

namespace SFA.DAS.EmployerFinance.Models
{
    public class AccountPayeScheme : Entity
    {
        public virtual long Id { get; internal set; }
        public virtual Account Account { get; internal set; }
        public virtual long AccountId { get; internal set; }
        public virtual string EmployerReferenceNumber { get; internal set; }
        public virtual DateTime Created { get; internal set; }
        public virtual DateTime? Deleted { get; internal set; }
        
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

        internal virtual void Delete(DateTime deleted)
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