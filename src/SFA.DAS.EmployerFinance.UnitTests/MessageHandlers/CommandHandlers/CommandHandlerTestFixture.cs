using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using NServiceBus;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    public class CommandHandlerTestFixture<TCommand, TCommandHandler> where TCommand : IRequest
                                                                      where TCommandHandler : IHandleMessages<TCommand>
    {
        public TCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<TCommand> Handler { get; set; }

        public CommandHandlerTestFixture()
        {
            var fixture = new Fixture();
            Command = fixture.Create<TCommand>();
            
            Mediator = new Mock<IMediator>();
            Handler = ConstructHandler();
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
        
        private TCommandHandler ConstructHandler()
        {
            return (TCommandHandler)Activator.CreateInstance(typeof(TCommandHandler), Mediator.Object);
        }
        
        public void VerifySend()
        {
            Mediator.Verify(m => m.Send(Command, CancellationToken.None), Times.Once);
        }
    }
}