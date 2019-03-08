using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
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
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(
                It.Is<UpdateLevyDeclarationTransactionBalancesCommand>(c => c.SagaId == f.Event.SagaId),
                CancellationToken.None), Times.Once));
        }
    }

    public class UpdatedLevyDeclarationSagaProgressEventHandlerTestsFixture
    {
        public UpdatedLevyDeclarationSagaProgressEvent Event { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<UpdatedLevyDeclarationSagaProgressEvent> Handler { get; set; }

        public UpdatedLevyDeclarationSagaProgressEventHandlerTestsFixture()
        {
            Event = new UpdatedLevyDeclarationSagaProgressEvent(1);
            Mediator = new Mock<IMediator>();
            Handler = new UpdatedLevyDeclarationSagaProgressEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Event, null);
        }
    }
}