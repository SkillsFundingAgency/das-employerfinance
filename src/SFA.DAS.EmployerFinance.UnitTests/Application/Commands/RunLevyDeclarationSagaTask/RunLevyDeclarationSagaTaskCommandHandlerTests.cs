using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.RunLevyDeclarationSagaTask;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountTransactionBalances;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.RunLevyDeclarationSagaTask
{
    [TestFixture]
    [Parallelizable]
    public class RunLevyDeclarationSagaTaskCommandHandlerTests : FluentTest<RunProcessLevyDeclarationsSagaTaskCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingImportPayeSchemeLevyDeclarationsCommand_ThenShouldCreateSagaTask()
        {
            return TestAsync(f => f.HandleImportPayeSchemeLevyDeclarationsCommand(), f => f.Db.LevyDeclarationSagaTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSagaTask>(t =>
                    t.SagaId == f.ImportPayeSchemeLevyDeclarationsCommand.SagaId &&
                    t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations &&
                    t.AccountPayeSchemeId == f.ImportPayeSchemeLevyDeclarationsCommand.AccountPayeSchemeId &&
                    t.AccountId == null &&
                    t.Started >= f.Now &&
                    t.Finished >= f.NextTaskInvoked));
        }
        
        [Test]
        public Task Handle_WhenHandlingImportPayeSchemeLevyDeclarationsCommand_ThenShouldInvokeNextTask()
        {
            return TestAsync(f => f.HandleImportPayeSchemeLevyDeclarationsCommand(), f => f.NextTask.Verify(c => c(), Times.Once()));
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdateAccountLevyDeclarationTransactionBalancesCommand_ThenShouldCreateSagaTask()
        {
            return TestAsync(f => f.HandleUpdateAccountLevyDeclarationTransactionBalancesCommand(), f => f.Db.LevyDeclarationSagaTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSagaTask>(t =>
                    t.SagaId == f.ImportPayeSchemeLevyDeclarationsCommand.SagaId &&
                    t.Type == LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances &&
                    t.AccountPayeSchemeId == null &&
                    t.AccountId == f.UpdateAccountLevyDeclarationTransactionBalancesCommand.AccountId &&
                    t.Started >= f.Now &&
                    t.Finished >= f.NextTaskInvoked));
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdateAccountLevyDeclarationTransactionBalancesCommand_ThenShouldInvokeNextTask()
        {
            return TestAsync(f => f.HandleUpdateAccountLevyDeclarationTransactionBalancesCommand(), f => f.NextTask.Verify(c => c(), Times.Once()));
        }
    }

    public class RunProcessLevyDeclarationsSagaTaskCommandHandlerTestsFixture
    {
        public DateTime Now { get; set; }
        public int SagaId { get; set; }
        public long AccountPayeSchemeId { get; set; }
        public long AccountId { get; set; }
        public ImportPayeSchemeLevyDeclarationsCommand ImportPayeSchemeLevyDeclarationsCommand { get; set; }
        public UpdateAccountLevyDeclarationTransactionBalancesCommand UpdateAccountLevyDeclarationTransactionBalancesCommand { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public RunLevyDeclarationSagaTaskCommandHandler Handler { get; set; }
        public Mock<RequestHandlerDelegate<Unit>> NextTask { get; set; }
        public DateTime NextTaskInvoked { get; set; }

        public RunProcessLevyDeclarationsSagaTaskCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            AccountPayeSchemeId = 1;
            AccountId = 2;
            ImportPayeSchemeLevyDeclarationsCommand = new ImportPayeSchemeLevyDeclarationsCommand(SagaId, Now, AccountPayeSchemeId);
            UpdateAccountLevyDeclarationTransactionBalancesCommand = new UpdateAccountLevyDeclarationTransactionBalancesCommand(SagaId, AccountId);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new RunLevyDeclarationSagaTaskCommandHandler(Db);
            NextTask = new Mock<RequestHandlerDelegate<Unit>>();
            
            NextTask.Setup(t => t()).Callback(() => NextTaskInvoked = DateTime.UtcNow).ReturnsAsync(Unit.Value);
        }

        public async Task HandleImportPayeSchemeLevyDeclarationsCommand()
        {
            await Handler.Handle(ImportPayeSchemeLevyDeclarationsCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }

        public async Task HandleUpdateAccountLevyDeclarationTransactionBalancesCommand()
        {
            await Handler.Handle(UpdateAccountLevyDeclarationTransactionBalancesCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }
    }
}