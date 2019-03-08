using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.Testing;
using ImportPayeSchemeLevyDeclarationsCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.ImportPayeSchemeLevyDeclarationsCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class ImportPayeSchemeLevyDeclarationsCommandHandlerTests : FluentTest<ImportPayeSchemeLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class ImportPayeSchemeLevyDeclarationsCommandHandlerTestsFixture
    {
        public ImportPayeSchemeLevyDeclarationsCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<ImportPayeSchemeLevyDeclarationsCommand> Handler { get; set; }

        public ImportPayeSchemeLevyDeclarationsCommandHandlerTestsFixture()
        {
            Command = new ImportPayeSchemeLevyDeclarationsCommand(1, DateTime.UtcNow, 1);
            Mediator = new Mock<IMediator>();
            Handler = new ImportPayeSchemeLevyDeclarationsCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}