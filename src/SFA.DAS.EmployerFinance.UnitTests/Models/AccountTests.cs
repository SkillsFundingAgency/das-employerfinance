using System;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

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
            Test(f => f.UpdatedWithFutureUpdate(), f => f.UpdateName(), f => f.AssertNameAndUpdatedDateNotUpdated());
        }
        
        [Test]
        public void UpdateName_WhenAccountNotPreviouslyUpdatedAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdateName(f.Account.Name), f => f.AssertNameAndUpdatedDateNotUpdated());
        }

        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasLaterUpdatedTimeAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedPreviously(), f => f.UpdateName(f.Account.Name),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }
        
        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasEarlierUpdatedTimeAndNewNameIsSameAsOldName_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedWithFutureUpdate(), f => f.UpdateName(f.Account.Name),
                f => f.AssertNameAndUpdatedDateNotUpdated());
        }

        [Test]
        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeShouldBeAddedToTheAccount()
        {
            Test(f => f.AddPayeScheme(), f => f.AssertPayeSchemeAdded());
        }

        [Test]
        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeShouldBeReturnedAndBeSameAsPayeSchemeAddedToAccount()
        {
            Test(f => f.AddPayeScheme(), (f, r) => f.AssertPayeSchemeIsReturnedAndSameAsPayeSchemeAdded(r));
        }
        
        [Test]
        public void AddPayeScheme_WhenPayeSchemeHasAlreadyBeenAddedToTheAccount_ThenShouldThrowInvalidOperationException()
        {
            TestException(f => f.AddPreExistingPayeScheme(), f => f.AddPayeScheme(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
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

        public AccountTestsFixture AddPreExistingPayeScheme()
        {
            Account._accountPayeSchemes.Add(new AccountPayeScheme(Account.Id, EmployerReferenceNumber, DateTime.UtcNow));
            return this;
        }
        
        #endregion Arrange

        #region Act
        
        public void UpdateName(string name = NewName)
        {
            CloneOriginalAccount();
            Account.UpdateName(name, ActionDate);
        }

        public AccountPayeScheme AddPayeScheme(string employerReferenceNumber = EmployerReferenceNumber)
        {
            CloneOriginalAccount();
            return Account.AddPayeScheme(employerReferenceNumber, ActionDate);
        }
        
        #endregion Act

        #region Assert

        public AccountTestsFixture AssertNameAndUpdatedDateUpdated()
        {
            Account.Name.Should().Be(NewName);
            Account.Updated.Should().Be(ActionDate);
            return this;
        }

        public AccountTestsFixture AssertNameAndUpdatedDateNotUpdated()
        {
            Account.Name.Should().Be(OriginalAccount.Name);
            Account.Updated.Should().Be(OriginalAccount.Updated);
            return this;
        }

        public AccountTestsFixture AssertPayeSchemeAdded()
        {
            Account.AccountPayeSchemes.Should().BeEquivalentTo(new AccountPayeScheme(OriginalAccount.Id, EmployerReferenceNumber, ActionDate));
            return this;
        }

        public AccountTestsFixture AssertPayeSchemeIsReturnedAndSameAsPayeSchemeAdded(AccountPayeScheme result)
        {
            result.Should().NotBeNull();
            Account.AccountPayeSchemes.Should().BeEquivalentTo(new AccountPayeScheme(OriginalAccount.Id, EmployerReferenceNumber, ActionDate));
            return this;
        }
        
        #endregion Assert
    }
}