using System.Threading.Tasks;
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
            return TestAsync(f => f.Handle(),f => f.VerifySend());
        }
    }

    public class ProcessLevyDeclarationsAdHocCommandHandlerTestsFixture : CommandHandlerTestFixture<ProcessLevyDeclarationsAdHocCommand, ProcessLevyDeclarationsAdHocCommandHandler>
    {
    }
}