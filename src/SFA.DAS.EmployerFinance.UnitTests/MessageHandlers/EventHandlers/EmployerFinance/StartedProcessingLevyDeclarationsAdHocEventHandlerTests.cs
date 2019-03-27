using System.Threading.Tasks;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EventHandlers.EmployerFinance
{
    [TestFixture]
    [Parallelizable]
    public class StartedProcessingLevyDeclarationsAdHocEventHandlerTests : FluentTest<StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendImportLevyDeclarationsCommand()
        {
            return TestAsync(f => f.Handle(), f => f.VerifySend<ImportPayeSchemeLevyDeclarationsCommand>((c, m) =>
                c.SagaId == m.SagaId &&
                c.PayrollPeriod == m.PayrollPeriod &&
                c.AccountPayeSchemeId == m.AccountPayeSchemeId));
        }
        
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateLevyDeclarationSagaProgressCommand()
        {
            return TestAsync(f => f.Handle(), f => f.AssertSentMessage<UpdateLevyDeclarationSagaProgressCommand>((sm, m) => sm.SagaId == m.SagaId));
        }
    }

    public class StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture : EventHandlerTestsFixture<StartedProcessingLevyDeclarationsAdHocEvent, StartedProcessingLevyDeclarationsAdHocEventHandler>
    {
    }
}