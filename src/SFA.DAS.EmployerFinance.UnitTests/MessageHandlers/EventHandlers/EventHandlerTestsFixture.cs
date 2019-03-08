using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Moq;
using NServiceBus;
using NServiceBus.Testing;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.EventHandlers
{
    public class EventHandlerTestsFixture<TEvent, TEventHandler> where TEventHandler : IHandleMessages<TEvent>
    {
        public Mock<IMediator> Mediator { get; set; }
        public TEvent Message { get; set; }
        public IHandleMessages<TEvent> Handler { get; set; }
        public TestableMessageHandlerContext MessageHandlerContext { get; set; }

        public EventHandlerTestsFixture(Func<IMediator, IHandleMessages<TEvent>> constructHandler = null)
        {
            Mediator = new Mock<IMediator>();

            var fixture = new Fixture();
            Message = fixture.Create<TEvent>();

            MessageHandlerContext = new TestableMessageHandlerContext();

            Handler = constructHandler != null ? constructHandler(Mediator.Object) : ConstructHandler();
        }

        public virtual Task Handle()
        {
            return Handler.Handle(Message, MessageHandlerContext);
        }

        private TEventHandler ConstructHandler()
        {
            return (TEventHandler)Activator.CreateInstance(typeof(TEventHandler), Mediator.Object);
        }

        public void VerifySend<TCommand>(Func<TCommand,TEvent,bool> verifyCommand) where TCommand : IRequest
        {
            Mediator.Verify(m => m.Send(It.Is<TCommand>(c => verifyCommand(c,Message)), CancellationToken.None), Times.Once);
        }

        public void AssertSentMessage<TMessage>(Func<TMessage,TEvent,bool> verifySentMessage)
        {
            MessageHandlerContext.SentMessages.Select(m => m.Message).SingleOrDefault().Should().NotBeNull()
                .And.Match<TMessage>(sentMessage => verifySentMessage(sentMessage, Message));
        }
    }
}