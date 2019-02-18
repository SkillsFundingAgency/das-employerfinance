using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.Testing;
using ImportLevyDeclarationsCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ImportLevyDeclarationsCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ImportLevyDeclarationsCommandHandlerTests : FluentTest<ImportLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldForwardCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ImportLevyDeclarationsCommandHandlerTestsFixture
    {
        public ImportLevyDeclarationsCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ImportLevyDeclarationsCommand> Handler { get; set; }

        public ImportLevyDeclarationsCommandHandlerTestsFixture()
        {
            Command = new ImportLevyDeclarationsCommand(Guid.NewGuid(), DateTime.UtcNow, 1);
            Mediator = new Mock<IMediator>();
            Handler = new ImportLevyDeclarationsCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}