using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using NServiceBus;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances;
using SFA.DAS.Testing;
using UpdateAccountTransactionBalancesCommandHandler = SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers.UpdateAccountTransactionBalancesCommandHandler;

namespace SFA.DAS.EmployerFinance.UnitTests.MessageHandlers.CommandHandlers
{
    [TestFixture]
    [Parallelizable]
    public class UpdateAccountTransactionBalancesCommandHandlerTests : FluentTest<UpdateAccountTransactionBalancesCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommand()
        {
            return TestAsync(f => f.Handle(),f => f.Mediator.Verify(m => m.Send(f.Command, CancellationToken.None), Times.Once));
        }
    }

    public class UpdateAccountTransactionBalancesCommandHandlerTestsFixture
    {
        public UpdateAccountLevyDeclarationTransactionBalancesCommand Command { get; set; }
        public Mock<IMediator> Mediator { get; set; }
        public IHandleMessages<UpdateAccountTransactionBalancesCommand> Handler { get; set; }

        public UpdateAccountTransactionBalancesCommandHandlerTestsFixture()
        {
            Command = new UpdateAccountLevyDeclarationTransactionBalancesCommand(1, 2);
            Mediator = new Mock<IMediator>();
            Handler = new UpdateAccountTransactionBalancesCommandHandler(Mediator.Object);
        }

        public Task Handle()
        {
            return Handler.Handle(Command, null);
        }
    }
}