using System;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    /// <summary>
    /// Tests both Account & AccountPayeScheme models.
    /// Usually would have an AccountsTest with a mocked AccountPayeScheme, and a separate AccountPayeSchemeTest,
    /// but it is more convenient in this instance to test them together.
    /// </summary>
    [TestFixture]
    [Parallelizable]
    public class AccountAndAccountPayeSchemeTests : FluentTest<AccountTestsFixture>
    {
        #region UpdateName

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

        #endregion UpdateName

        #region AddPayeScheme
        
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
        
        #endregion AddPayeScheme

        #region RemovePayeScheme

        [Test]
        public void RemovePayeScheme_WhenPayeSchemeHasAlreadyBeenAddedToTheAccount_ThenPayeSchemeShouldBeSoftDeletedFromTheAccount()
        {
            Test(f => f.AddPreExistingPayeScheme(), f => f.RemovePayeScheme(), f => f.AssertPayeSchemeSoftDeleted());
        }

        [Test]
        public void RemovePayeScheme_WhenPayeSchemeHasntAlreadyBeenAddedToTheAccount_ThenShouldThrowInvalidOperationException()
        {
            TestException(f => f.RemovePayeScheme(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }

        #endregion RemovePayeScheme
    }

    public class AccountTestsFixture
    {
        public Account Account { get; set; }
        public Account OriginalAccount { get; set; }
        public const string NewName = "NewName";
        public const string EmployerReferenceNumber = "ABC/123456";
        public DateTime ActionDate { get; set; }
        public AccountPayeScheme AccountPayeScheme { get; set; }
        public AccountPayeScheme OriginalAccountPayeScheme { get; set; }
        private readonly Fixture _fixture;
        
        #region Arrange
        
        public AccountTestsFixture()
        {
            _fixture = new Fixture();
            Account = _fixture.Create<Account>();
            ActionDate = _fixture.Create<DateTime>();
            AccountPayeScheme = new AccountPayeScheme(Account.Id, EmployerReferenceNumber, ActionDate.AddMinutes(-1));
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

        public AccountTestsFixture CloneOriginalAccountPayeScheme()
        {
            //doesn't work for some reason
            //OriginalAccountPayeScheme = JsonConvert.DeserializeObject<AccountPayeScheme>(JsonConvert.SerializeObject(AccountPayeScheme));
            OriginalAccountPayeScheme = new AccountPayeScheme(AccountPayeScheme.AccountId, AccountPayeScheme.EmployerReferenceNumber, AccountPayeScheme.Created);
            return this;
        }
        
        public AccountTestsFixture AddPreExistingPayeScheme()
        {
            Account._accountPayeSchemes.Add(AccountPayeScheme);
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

        public void RemovePayeScheme()
        {
            CloneOriginalAccount();
            CloneOriginalAccountPayeScheme();
            Account.RemovePayeScheme(AccountPayeScheme, ActionDate);
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

        public AccountTestsFixture AssertPayeSchemeSoftDeleted()
        {
            OriginalAccountPayeScheme.Deleted = ActionDate;
            Account.AccountPayeSchemes.Should().ContainEquivalentOf(OriginalAccountPayeScheme);
            return this;
        }
        
        #endregion Assert
    }
}