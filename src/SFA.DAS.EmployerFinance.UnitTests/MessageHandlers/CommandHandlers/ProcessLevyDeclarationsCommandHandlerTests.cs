using System.Threading.Tasks;
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
            return TestAsync(f => f.Handle(),f => f.VerifySend());
        }
    }

    public class ProcessLevyDeclarationsCommandHandlerTestsFixture : CommandHandlerTestFixture<ProcessLevyDeclarationsCommand, ProcessLevyDeclarationsCommandHandler>
    {
    }
}