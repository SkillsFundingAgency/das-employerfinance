using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.AddAccount;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class CreatedAccountEventHandlerTests : FluentTest<CreatedAccountEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEvent_ThenShouldSendAddAccountCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<AddAccountCommand>((c, m) => 
                c.Id == m.AccountId &&
                c.HashedId == m.HashedId &&
                c.PublicHashedId == m.PublicHashedId &&
                c.Name == m.Name &&
                c.Added == m.Created));
        }
    }

    public class CreatedAccountEventHandlerTestsFixture : EventHandlerTestsFixture<CreatedAccountEvent, CreatedAccountEventHandler>
    {
    }
}