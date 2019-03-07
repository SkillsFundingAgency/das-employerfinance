using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.Testing;
using ProcessLevyDeclarationsCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ProcessLevyDeclarationsCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsCommandHandlerTests : FluentTest<ProcessLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ProcessLevyDeclarationsCommandHandlerTestsFixture
    {
        public ProcessLevyDeclarationsCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ProcessLevyDeclarationsCommand> Handler { get; set; }

        public ProcessLevyDeclarationsCommandHandlerTestsFixture()
        {
            Command = new ProcessLevyDeclarationsCommand(DateTime.UtcNow);
            Mediator = new Mock<IMediator>();
            Handler = new ProcessLevyDeclarationsCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}