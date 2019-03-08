using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
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
            //todo: VerifySend
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(
                It.Is<ImportPayeSchemeLevyDeclarationsCommand>(c => 
                    c.SagaId == f.Message.SagaId &&
                    c.PayrollPeriod == f.Message.PayrollPeriod &&
                    c.AccountPayeSchemeId == f.Message.AccountPayeSchemeId),
                CancellationToken.None), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateLevyDeclarationSagaProgressCommand()
        {
            return TestAsync(f => f.Handle(), f => f.MessageHandlerContext.SentMessages.Select(m => m.Message).SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Message.SagaId));
        }
    }

    public class StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture : EventHandlerTestsFixture<StartedProcessingLevyDeclarationsAdHocEvent, StartedProcessingLevyDeclarationsAdHocEventHandler>
    {
    }
}