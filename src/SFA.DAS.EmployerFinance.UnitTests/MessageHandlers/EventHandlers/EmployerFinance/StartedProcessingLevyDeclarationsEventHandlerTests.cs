using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EventHandlers.EmployerFinance
{
    [TestFixture]
    [Parallelizable]
    public class StartedProcessingLevyDeclarationsEventHandlerTests : FluentTest<StartedProcessingLevyDeclarationsEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<ImportLevyDeclarationsCommand>((c, m) =>
                c.SagaId == m.SagaId &&
                c.PayrollPeriod == m.PayrollPeriod &&
                c.AccountPayeSchemeHighWaterMarkId == m.AccountPayeSchemeHighWaterMarkId));
        }
        
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateLevyDeclarationSagaProgressCommand()
        {
            return TestAsync(f => f.Handle(), f => f.AssertSentMessage<UpdateLevyDeclarationSagaProgressCommand>((sm, m) => sm.SagaId == m.SagaId));
        }
    }

    public class StartedProcessingLevyDeclarationsEventHandlerTestsFixture : EventHandlerTestsFixture<StartedProcessingLevyDeclarationsEvent, StartedProcessingLevyDeclarationsEventHandler>
    {
    }
}