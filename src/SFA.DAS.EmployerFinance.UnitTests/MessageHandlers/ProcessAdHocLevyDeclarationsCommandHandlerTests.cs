using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessAdHocLevyDeclarations;
using SFA.DAS.Testing;
using ProcessAdHocLevyDeclarationsCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ProcessAdHocLevyDeclarationsCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ProcessAdHocLevyDeclarationsCommandHandlerTests : FluentTest<ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldForwardCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture
    {
        public ProcessAdHocLevyDeclarationsCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ProcessAdHocLevyDeclarationsCommand> Handler { get; set; }

        public ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture()
        {
            Command = new ProcessAdHocLevyDeclarationsCommand(DateTime.UtcNow, 1);
            Mediator = new Mock<IMediator>();
            Handler = new ProcessAdHocLevyDeclarationsCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}