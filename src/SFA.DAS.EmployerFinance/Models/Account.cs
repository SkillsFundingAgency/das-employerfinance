using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.EmployerFinance.Models
{
    public class Account : Entity
    {
        public virtual long Id { get; internal set; }
        public virtual string HashedId { get; internal set; }
        public virtual string PublicHashedId { get; internal set; }
        public virtual string Name { get; internal set; }
        public virtual DateTime Created { get; internal set; }
        public virtual DateTime? Updated { get; internal set; }
        public virtual IEnumerable<AccountPayeScheme> AccountPayeSchemes => _accountPayeSchemes;
        
        internal readonly List<AccountPayeScheme> _accountPayeSchemes = new List<AccountPayeScheme>();

        public Account(long id, string hashedId, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            HashedId = hashedId;
            PublicHashedId = publicHashedId;
            Name = name;
            Created = created;
        }

        internal Account()
        {
        }
        
        public virtual void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                Name = name;
                Updated = updated;
            }
        }

        public virtual AccountPayeScheme AddPayeScheme(string employerReferenceNumber, DateTime created)
        {
            EnsurePayeSchemeHasNotAlreadyBeenAdded(employerReferenceNumber);
            
            var accountPayeScheme = new AccountPayeScheme(this, employerReferenceNumber, created);
            
            _accountPayeSchemes.Add(accountPayeScheme);

            return accountPayeScheme;
        }

        public virtual void RemovePayeScheme(AccountPayeScheme accountPayeScheme, DateTime removed)
        {
            EnsureAccountPayeSchemeHasBeenAdded(accountPayeScheme);
            
            accountPayeScheme.Delete(removed);
        }

        private void EnsureAccountPayeSchemeHasBeenAdded(AccountPayeScheme accountPayeScheme)
        {
            if (_accountPayeSchemes.All(aps => aps.AccountId != accountPayeScheme.AccountId && aps.EmployerReferenceNumber != accountPayeScheme.EmployerReferenceNumber))
            {
                throw new InvalidOperationException("Requires account paye scheme has been added");
            }
        }

        private void EnsurePayeSchemeHasNotAlreadyBeenAdded(string employerReferenceNumber)
        {
            if (_accountPayeSchemes.Any(aps => aps.EmployerReferenceNumber == employerReferenceNumber))
            {
                throw new InvalidOperationException("Requires account paye scheme has not already been added");
            }
        }

        private bool IsUpdatedNameDateChronological(DateTime updated)
        {
            return Updated == null || updated > Updated.Value;
        }

        private bool IsUpdatedNameDifferent(string name)
        {
            return name != Name;
        }
    }
}