using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;
using SFA.DAS.Testing;
using UpdateAccountBalanceCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.UpdateAccountBalanceCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers
{
    [TestFixture]
    [Parallelizable]
    public class UpdateAccountBalanceCommandHandlerTests : FluentTest<UpdateAccountBalanceCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldForwardCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class UpdateAccountBalanceCommandHandlerTestsFixture
    {
        public UpdateAccountBalanceCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<UpdateAccountBalanceCommand> Handler { get; set; }

        public UpdateAccountBalanceCommandHandlerTestsFixture()
        {
            Command = new UpdateAccountBalanceCommand(Guid.NewGuid(), 1);
            Mediator = new Mock<IMediator>();
            Handler = new UpdateAccountBalanceCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}