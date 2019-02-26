using System;
using System.Collections.Generic;
using System.Linq;

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

        public Account(long id, string hashedId, string publicHashedId, string name, DateTime created)
        {
            Id = id;
            HashedId = hashedId;
            PublicHashedId = publicHashedId;
            Name = name;
            Created = created;
        }

        private Account()
        {
        }
        
        public void UpdateName(string name, DateTime updated)
        {
            if (IsUpdatedNameDateChronological(updated) && IsUpdatedNameDifferent(name))
            {
                Name = name;
                Updated = updated;
            }
        }

        public AccountPayeScheme AddPayeScheme(string employerReferenceNumber, DateTime created)
        {
            EnsurePayeSchemeHasNotAlreadyBeenAdded(employerReferenceNumber);
            
            var accountPayeScheme = new AccountPayeScheme(Id, employerReferenceNumber, created);
            
            _accountPayeSchemes.Add(accountPayeScheme);

            return accountPayeScheme;
        }

        public void RemovePayeScheme(AccountPayeScheme accountPayeScheme, DateTime removed)
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
            //todo: this will require a db round-trip, better to catch error if it already exists?
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