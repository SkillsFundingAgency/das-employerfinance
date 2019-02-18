using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFinance.Models
{
    public class PayeScheme
    {
        public string EmployerReferenceNumber { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime? Updated { get; private set; }
        public DateTime? Deleted { get; private set; }
        public IEnumerable<AccountPayeeScheme> AccountPayeeSchemes => _accountPayeeScheme;

        private readonly List<AccountPayeeScheme> _accountPayeeScheme = new List<AccountPayeeScheme>();
        
        public PayeScheme(string employerReferenceNumber, string name, DateTime created)
        {
            EmployerReferenceNumber = employerReferenceNumber;
            Name = name;
            Created = created;
        }

        private PayeScheme()
        {
        }
        
        public void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                EnsureHasNotBeenDeleted();

                Name = name;
                Updated = updated;
            }
        }
        
        internal void Delete(DateTime deleted)
        {
            EnsureHasNotBeenDeleted();
            
            foreach (var accountPayeeScheme in _accountPayeeScheme)
            {
                accountPayeeScheme.Delete(deleted);
            }
            
            _accountPayeeScheme.Clear();
            
            Deleted = deleted;
        }

        private void EnsureHasNotBeenDeleted()
        {
            if (Deleted != null)
            {
                throw new InvalidOperationException("Requires payee scheme has not been deleted");
            }
        }
        
        private bool IsUpdatedNameDateChronological(DateTime updated)
        {
            return (Updated == null || updated > Updated.Value) && (Deleted == null || updated > Deleted.Value);
        }

        private bool IsUpdatedNameDifferent(string name)
        {
            return name != Name;
        }
    }
}