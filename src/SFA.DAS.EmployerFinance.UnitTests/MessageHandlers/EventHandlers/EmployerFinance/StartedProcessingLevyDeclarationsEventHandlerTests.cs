using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using NServiceBus;
using NServiceBus.Testing;
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
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(
                It.Is<ImportLevyDeclarationsCommand>(c =>
                    c.SagaId == f.Event.SagaId &&
                    c.PayrollPeriod == f.Event.PayrollPeriod &&
                    c.AccountPayeSchemeHighWaterMarkId == f.Event.AccountPayeSchemeHighWaterMarkId),
                CancellationToken.None), Times.Once));
        }
        
        [Test]
        public Task Handle_WhenHandlingEvent_ThenShouldSendUpdateLevyDeclarationSagaProgressCommand()
        {
            return TestAsync(f => f.Handle(), f => f.Context.SentMessages.Select(m => m.Message).SingleOrDefault().Should().NotBeNull()
                .And.Match<UpdateLevyDeclarationSagaProgressCommand>(c => c.SagaId == f.Event.SagaId));
        }
    }

    public class StartedProcessingLevyDeclarationsEventHandlerTestsFixture
    {
        public StartedProcessingLevyDeclarationsEvent Event { get; set; }

        public TestableMessageHandlerContext Context { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<StartedProcessingLevyDeclarationsEvent> Handler { get; set; }

        public StartedProcessingLevyDeclarationsEventHandlerTestsFixture()
        {
            Event = new StartedProcessingLevyDeclarationsEvent(1, DateTime.UtcNow.AddMonths(-1), 2, DateTime.UtcNow);
            Context = new TestableMessageHandlerContext();
            Mediator = new Mock<IMediator>();
            Handler = new StartedProcessingLevyDeclarationsEventHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Event, Context);
        }
    }
}