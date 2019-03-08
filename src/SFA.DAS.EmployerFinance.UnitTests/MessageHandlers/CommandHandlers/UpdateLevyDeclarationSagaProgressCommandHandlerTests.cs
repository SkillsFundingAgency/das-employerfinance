using System.Threading.Tasks;
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
            return TestAsync(f => f.Handle(),f => f.VerifySend());
        }
    }

    public class UpdateLevyDeclarationSagaProgressCommandHandlerTestsFixture : CommandHandlerTestFixture<UpdateLevyDeclarationSagaProgressCommand, UpdateLevyDeclarationSagaProgressCommandHandler>
    {
    }
}