using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationSagaProgress;
using SFA.DAS.Testing;
using UpdateLevyDeclarationSagaProgressCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.UpdateLevyDeclarationSagaProgressCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class UpdateLevyDeclarationSagaProgressCommandHandlerTests : FluentTest<UpdateLevyDeclarationSagaProgressCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class UpdateLevyDeclarationSagaProgressCommandHandlerTestsFixture
    {
        public UpdateLevyDeclarationSagaProgressCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<UpdateLevyDeclarationSagaProgressCommand> Handler { get; set; }

        public UpdateLevyDeclarationSagaProgressCommandHandlerTestsFixture()
        {
            Command = new UpdateLevyDeclarationSagaProgressCommand(1);
            Mediator = new Mock<IMediator>();
            Handler = new UpdateLevyDeclarationSagaProgressCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}