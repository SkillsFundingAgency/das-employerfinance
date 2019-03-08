using System.Threading.Tasks;
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
            return TestAsync(f => f.Handle(),f => f.VerifySend());
        }
    }

    public class ImportPayeSchemeLevyDeclarationsCommandHandlerTestsFixture : CommandHandlerTestFixture<ImportPayeSchemeLevyDeclarationsCommand, ImportPayeSchemeLevyDeclarationsCommandHandler>
    {
    }
}