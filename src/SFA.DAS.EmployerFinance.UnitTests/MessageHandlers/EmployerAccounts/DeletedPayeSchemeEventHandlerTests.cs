using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Messages.Events;
using SFA.DAS.EmployerFinance.Application.Commands.RemoveAccountPayeScheme;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerAccounts;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EmployerAccounts
{
    [TestFixture]
    [Parallelizable]
    public class DeletedPayeSchemeEventHandlerTests : FluentTest<DeletedPayeSchemeEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingDeletedPayeSchemeEvent_ThenShouldSendRemoveAccountPayeSchemeCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<RemoveAccountPayeSchemeCommand>((c, m) => 
                c.AccountId == m.AccountId &&
                c.EmployerReferenceNumber == m.PayeRef &&
                c.Removed == m.Created));
        }
    }

    public class DeletedPayeSchemeEventHandlerTestsFixture : EventHandlerTestsFixture<DeletedPayeSchemeEvent, DeletedPayeSchemeEventHandler>
    {
    }
}