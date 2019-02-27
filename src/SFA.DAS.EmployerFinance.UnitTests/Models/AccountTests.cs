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
    public class AccountTests : FluentTest<AccountTestsFixture>
    {
        [Test]
        public void UpdateName_WhenAccountNotPreviouslyUpdated_ThenAccountShouldBeUpdated()
        {
            Test(f => f.Account.UpdateName(f.NewName, f.ActionDate), f =>
            {
                f.Account.Name.Should().Be(f.NewName);
                f.Account.Updated.Should().Be(f.ActionDate);
            });
        }

        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasLaterUpdatedTime_ThenAccountShouldBeUpdated()
        {
            Test(f => f.PreviousUpdate(), f => f.Account.UpdateName(f.NewName, f.ActionDate), f =>
            {
                f.Account.Name.Should().Be(f.NewName);
                f.Account.Updated.Should().Be(f.ActionDate);
            });
        }
    }

    public class AccountTestsFixture
    {
        public Account Account { get; set; }
        public string NewName { get; set; }
        public DateTime ActionDate { get; set; }
        public Fixture Fixture { get; set; }
        
        public AccountTestsFixture()
        {
            Fixture = new Fixture();
            Account = Fixture.Create<Account>();
            ActionDate = Fixture.Create<DateTime>();
            NewName = Fixture.Create<string>();
        }

        public void PreviousUpdate()
        {
            Account.Updated = Fixture.Create<DateTime>();
            ActionDate = Account.Updated.Value.AddMinutes(1);
        }
    }
}