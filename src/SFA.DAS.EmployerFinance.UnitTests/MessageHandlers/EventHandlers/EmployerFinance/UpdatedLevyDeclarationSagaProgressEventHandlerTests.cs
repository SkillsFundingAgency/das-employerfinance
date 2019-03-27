using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EventHandlers.EmployerFinance
{
    [TestFixture]
    [Parallelizable]
    public class UpdatedLevyDeclarationSagaProgressEventHandlerTests : FluentTest<UpdatedLevyDeclarationSagaProgressEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<UpdateLevyDeclarationTransactionBalancesCommand>((c, m) => c.SagaId == m.SagaId));
        }
    }

    public class UpdatedLevyDeclarationSagaProgressEventHandlerTestsFixture : EventHandlerTestsFixture<UpdatedLevyDeclarationSagaProgressEvent, UpdatedLevyDeclarationSagaProgressEventHandler>
    {
    }
}