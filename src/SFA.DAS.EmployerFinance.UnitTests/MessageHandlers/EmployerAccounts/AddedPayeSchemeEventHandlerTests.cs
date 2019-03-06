using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.AddAccountPayeScheme;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class AddedPayeSchemeEventHandlerTests : FluentTest<AddedPayeSchemeEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCreatedAccountEventAddedPayeSchemeEvent_ThenShouldSendAddAccountPayeSchemeCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<AddAccountPayeSchemeCommand>((c, m) => 
                c.AccountId == m.AccountId &&
                c.EmployerReferenceNumber == m.PayeRef &&
                c.Added == m.Created));
        }
    }

    public class AddedPayeSchemeEventHandlerTestsFixture : EventHandlerTestsFixture<AddedPayeSchemeEvent, AddedPayeSchemeEventHandler>
    {
    }
}