using System;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using Fix = SFA.DAS.EmployerFinance.UnitTests.Models.AccountTestsFixture;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class AccountTests : FluentTest<AccountTestsFixture>
    {
        [Test]
        public void UpdateName_WhenAccountNotPreviouslyUpdated_ThenAccountShouldBeUpdated()
        {
            Test(f => f.UpdateName(), f => f.AssertNameAndUpdatedDateUpdated());
        }

        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasLaterUpdatedTime_ThenAccountShouldBeUpdated()
        {
            Test(f => f.UpdatedPreviously(), f => f.UpdateName(), f => f.AssertNameAndUpdatedDateUpdated());
        }
        
        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasEarlierUpdatedTime_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedWithFutureUpdate().CloneOriginalAccount(), f => f.UpdateName(),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }
        
        [Test]
        public void UpdateName_WhenAccountNotPreviouslyUpdatedAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.CloneOriginalAccount(),f => f.UpdateName(f.Account.Name),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }

        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasLaterUpdatedTimeAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedPreviously().CloneOriginalAccount(), f => f.UpdateName(f.Account.Name),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }
        
        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasEarlierUpdatedTimeAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedWithFutureUpdate().CloneOriginalAccount(), f => f.UpdateName(f.Account.Name),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }

        //todo: always clone before act?
        [Test]
        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeShouldBeAddedToTheAccount()
        {
            Test(f => f.CloneOriginalAccount(), f => f.AddPayeScheme(), f => f.AssertPayeSchemeAdded());
        }
        
//        [Test]
//        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeShouldBeReturned()
//        {
//            Test(f => f.AddPayeScheme(), f => f.);
//        }
//
//        [Test]
//        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeAddedToTheAccountShouldBeSameAsPayeSchemeReturned()
//        {
//            Test(f => f.AddPayeScheme(), f => f.);
//        }
    }

    public class AccountTestsFixture
    {
        public Account Account { get; set; }
        public Account OriginalAccount { get; set; }
        public const string NewName = "NewName";
        public const string EmployerReferenceNumber = "ABC/123456";
        public DateTime ActionDate { get; set; }
        private readonly Fixture _fixture;
        
        #region Arrange
        
        public AccountTestsFixture()
        {
            _fixture = new Fixture();
            Account = _fixture.Create<Account>();
            ActionDate = _fixture.Create<DateTime>();
        }
        
        public AccountTestsFixture UpdatedPreviously()
        {
            Account.Updated = _fixture.Create<DateTime>();
            ActionDate = Account.Updated.Value.AddMinutes(1);
            return this;
        }

        public AccountTestsFixture UpdatedWithFutureUpdate()
        {
            Account.Updated = _fixture.Create<DateTime>();
            ActionDate = Account.Updated.Value.AddMinutes(-1);
            return this;
        }

        public AccountTestsFixture CloneOriginalAccount()
        {
            OriginalAccount = JsonConvert.DeserializeObject<Account>(JsonConvert.SerializeObject(Account));
            OriginalAccount.Updated = Account.Updated;
            return this;
        }

        #endregion Arrange

        #region Act
        
        public void UpdateName(string name = NewName)
        {
            Account.UpdateName(name, ActionDate);
        }

        public void AddPayeScheme(string employerReferenceNumber = EmployerReferenceNumber)
        {
            Account.AddPayeScheme(employerReferenceNumber, ActionDate);
        }
        
        #endregion Act

        #region Assert

        public void AssertNameAndUpdatedDateUpdated()
        {
            Account.Name.Should().Be(NewName);
            Account.Updated.Should().Be(ActionDate);
        }

        public void AssertNameAndUpdatedDateNotUpdated()
        {
            Account.Name.Should().Be(OriginalAccount.Name);
            Account.Updated.Should().Be(OriginalAccount.Updated);
        }

        public void AssertPayeSchemeAdded()
        {
            Account.AccountPayeSchemes.Should().BeEquivalentTo(new AccountPayeScheme(OriginalAccount.Id, EmployerReferenceNumber, ActionDate));
        }
        
        #endregion Assert
    }
}