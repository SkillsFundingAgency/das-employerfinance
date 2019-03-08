using System;
using AutoFixture;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Models
{
    [TestFixture]
    [Parallelizable]
    public class AccountPayeSchemeTests : FluentTest<AccountPayeSchemeTestsFixture>
    {
        [Test]
        public void Delete_WhenAnAccountPayeSchemeIsDeleted_ThenItShouldBeMarkedAsDeleted()
        {
            Test(f => f.Delete(), f => f.AssertSoftDeleted());
        }

        [Test]
        public void Delete_WhenAnAlreadySoftDeletedAccountPayeSchemeIsDeleted_ThenAnExceptionShouldBeThrown()
        {
            TestException(f => f.Delete(), f => f.Delete(), (f, r) => r.Should().Throw<InvalidOperationException>());
        }
    }

    public class AccountPayeSchemeTestsFixture
    {
        public AccountPayeScheme AccountPayeScheme { get; set; }
        public Account Account { get; set; }
        public const string EmployerReferenceNumber = "ABC/123456";
        public DateTime ActionDate { get; set; }

        public AccountPayeSchemeTestsFixture()
        {
            var fixture = new Fixture();
            ActionDate = fixture.Create<DateTime>();
            Account = new Account(2017, "123456", "123456", "name", fixture.Create<DateTime>());
            
            AccountPayeScheme = new AccountPayeScheme(Account, EmployerReferenceNumber, ActionDate);
        }

        public void Delete()
        {
            AccountPayeScheme.Delete(ActionDate);
        }

        public void AssertSoftDeleted()
        {
            AccountPayeScheme.Deleted.Should().Be(ActionDate);
        }
    }
}