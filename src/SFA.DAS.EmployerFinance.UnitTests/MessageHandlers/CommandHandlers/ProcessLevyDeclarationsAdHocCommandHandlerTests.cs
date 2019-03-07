using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsAdHoc;
using SFA.DAS.Testing;
using ProcessLevyDeclarationsAdHocCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ProcessLevyDeclarationsAdHocCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsAdHocCommandHandlerTests : FluentTest<ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture
    {
        public ProcessLevyDeclarationsAdHocCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ProcessLevyDeclarationsAdHocCommand> Handler { get; set; }

        public ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture()
        {
            Command = new ProcessLevyDeclarationsAdHocCommand(DateTime.UtcNow, 1);
            Mediator = new Mock<IMediator>();
            Handler = new ProcessLevyDeclarationsAdHocCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}