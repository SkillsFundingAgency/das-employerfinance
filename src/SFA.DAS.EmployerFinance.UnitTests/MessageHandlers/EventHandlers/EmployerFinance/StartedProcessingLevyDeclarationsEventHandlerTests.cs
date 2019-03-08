using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
            return TestAsync(f => f.Handle(), f => f.MessageHandlerContext.SentMessages.Select(m => m.Message).SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Message.SagaId));
        }
    }

    public class StartedProcessingLevyDeclarationsEventHandlerTestsFixture : EventHandlerTestsFixture<StartedProcessingLevyDeclarationsEvent, StartedProcessingLevyDeclarationsEventHandler>
    {
    }
}