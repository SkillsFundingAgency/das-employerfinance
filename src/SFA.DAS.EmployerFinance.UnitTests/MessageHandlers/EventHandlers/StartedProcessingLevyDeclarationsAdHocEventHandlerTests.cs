using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance;
using SFA.DAS.EmployerFinance.Messages.Events;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EventHandlers
{
    [TestFixture]
    [Parallelizable]
    public class StartedProcessingLevyDeclarationsAdHocEventHandlerTests : FluentTest<StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendImportLevyDeclarationsCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(
                It.Is<ImportLevyDeclarationsCommand>(c => c.SagaId == f.Event.SagaId),
                CancellationToken.None), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateLevyDeclarationSagaProgressCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Event.SagaId),
                CancellationToken.None), Times.Once));
        }
    }

    public class StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture
    {
        public StartedProcessingLevyDeclarationsAdHocEvent Event { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<StartedProcessingLevyDeclarationsAdHocEvent> Handler { get; set; }

        public StartedProcessingLevyDeclarationsAdHocEventHandlerTestsFixture()
        {
            Event = new StartedProcessingLevyDeclarationsAdHocEvent(1, DateTime.UtcNow, 123, DateTime.UtcNow);
            Mediator = new Mock<IMediator>();
            Handler = new StartedProcessingLevyDeclarationsAdHocEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Event, null);
        }
    }
}