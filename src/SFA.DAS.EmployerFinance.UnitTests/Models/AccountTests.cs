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
            Test(f => f.Account.UpdateName(f.NewName, f.ActionDate), f =>
            {
                f.Account.Name.Should().Be(f.NewName);
                f.Account.Updated.Should().Be(f.ActionDate);
            });
        }

        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasLaterUpdatedTime_ThenAccountShouldBeUpdated()
        {
            Test(f => f.UpdatedPreviously(), f => f.Account.UpdateName(f.NewName, f.ActionDate), f =>
            {
                f.Account.Name.Should().Be(f.NewName);
                f.Account.Updated.Should().Be(f.ActionDate);
            });
        }
        
        [Test]
        public void UpdateName_WhenAccountPreviouslyUpdatedAndUpdateNameHasEarlierUpdatedTime_ThenAccountShouldNotBeUpdated()
        {
            Test(f => f.UpdatedWithFutureUpdate().CloneOriginalAccount(), f => f.Account.UpdateName(f.NewName, f.ActionDate), f =>
            {
                f.Account.Name.Should().Be(f.OriginalAccount.Name);
                f.Account.Updated.Should().Be(f.OriginalAccount.Updated);
            });
        }
    }

    public class AccountTestsFixture
    {
        public Account Account { get; set; }
        public Account OriginalAccount { get; set; }
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

        public AccountTestsFixture UpdatedPreviously()
        {
            Account.Updated = Fixture.Create<DateTime>();
            ActionDate = Account.Updated.Value.AddMinutes(1);
            return this;
        }

        public AccountTestsFixture UpdatedWithFutureUpdate()
        {
            Account.Updated = Fixture.Create<DateTime>();
            ActionDate = Account.Updated.Value.AddMinutes(-1);
            return this;
        }

        public AccountTestsFixture CloneOriginalAccount()
        {
            OriginalAccount = JsonConvert.DeserializeObject<Account>(JsonConvert.SerializeObject(Account));
            OriginalAccount.Updated = Account.Updated;
            return this;
        }
    }
}