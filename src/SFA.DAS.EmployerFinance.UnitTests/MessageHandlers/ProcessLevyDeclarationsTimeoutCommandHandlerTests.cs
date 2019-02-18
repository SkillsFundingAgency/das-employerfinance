using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;
using SFA.DAS.Testing;
using ProcessLevyDeclarationsTimeoutCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ProcessLevyDeclarationsTimeoutCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsTimeoutCommandHandlerTests : FluentTest<ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldForwardCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture
    {
        public ProcessLevyDeclarationsTimeoutCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ProcessLevyDeclarationsTimeoutCommand> Handler { get; set; }

        public ProcessLevyDeclarationsTimeoutCommandHandlerTestsFixture()
        {
            Command = new ProcessLevyDeclarationsTimeoutCommand(Guid.NewGuid());
            Mediator = new Mock<IMediator>();
            Handler = new ProcessLevyDeclarationsTimeoutCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}