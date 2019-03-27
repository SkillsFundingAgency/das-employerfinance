using System;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class AccountTests : FluentTest<AccountTestsFixture>
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
        public void AddPayeScheme_WhenPayeSchemeIsNewToTheAccount_ThenPayeSchemeShouldBeReturned()
        {
            Test(f => f.AddPayeScheme(), (f, r) => f.AssertCorrectPayeSchemeIsReturned(r));
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
        public Mock<AccountPayeScheme> AccountPayeScheme { get; set; }
        private readonly Fixture _fixture;
        
        #region Arrange
        
        public AccountTestsFixture()
        {
            _fixture = new Fixture();
            Account = _fixture.Create<Account>();
            ActionDate = _fixture.Create<DateTime>();
            AccountPayeScheme = new Mock<AccountPayeScheme>();
            AccountPayeScheme.SetupGet(aps => aps.AccountId).Returns(Account.Id);
            AccountPayeScheme.SetupGet(aps => aps.EmployerReferenceNumber).Returns(EmployerReferenceNumber);
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

        /// <summary>
        /// Clones public properties. Leaves _accountPayeSchemes alone: it's not public and will contain mock AccountPayeScheme, for which cloning makes no sense.
        /// </summary>
        public AccountTestsFixture PartCloneOriginalAccount()
        {
            OriginalAccount = new Account
            {
                Id = Account.Id,
                Name = Account.Name,
                Created = Account.Created,
                Updated = Account.Updated,
                HashedId = Account.HashedId,
                PublicHashedId = Account.PublicHashedId
            };
            return this;
        }

        public AccountTestsFixture AddPreExistingPayeScheme()
        {
            Account._accountPayeSchemes.Add(AccountPayeScheme.Object);
            return this;
        }
        
        #endregion Arrange

        #region Act
        
        public void UpdateName(string name = NewName)
        {
            PartCloneOriginalAccount();
            Account.UpdateName(name, ActionDate);
        }

        public AccountPayeScheme AddPayeScheme(string employerReferenceNumber = EmployerReferenceNumber)
        {
            PartCloneOriginalAccount();
            return Account.AddPayeScheme(employerReferenceNumber, ActionDate);
        }

        public void RemovePayeScheme()
        {
            PartCloneOriginalAccount();
            Account.RemovePayeScheme(AccountPayeScheme.Object, ActionDate);
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
            // assumes no transformation in AccountPayeScheme c'tor. can we remove that assumption, but still test that the correct AccountPayeScheme is added?
            Account.AccountPayeSchemes.Should().ContainSingle(aps => aps.AccountId == OriginalAccount.Id
                                                                     && aps.EmployerReferenceNumber == EmployerReferenceNumber
                                                                     && aps.Created == ActionDate);
            return this;
        }
        
        public AccountTestsFixture AssertCorrectPayeSchemeIsReturned(AccountPayeScheme result)
        {
            result.Should().NotBeNull();
            result.Should().Match<AccountPayeScheme>(aps => aps.AccountId == OriginalAccount.Id
                                         && aps.EmployerReferenceNumber == EmployerReferenceNumber
                                         && aps.Created == ActionDate);
            return this;
        }
        
        public AccountTestsFixture AssertPayeSchemeSoftDeleted()
        {
            AccountPayeScheme.Verify(aps => aps.Delete(ActionDate), Times.Once);
            Account.AccountPayeSchemes.Should().ContainSingle(aps => aps.AccountId == OriginalAccount.Id 
                                                                     && aps.EmployerReferenceNumber == EmployerReferenceNumber);
            return this;
        }
        
        #endregion Assert
    }
}