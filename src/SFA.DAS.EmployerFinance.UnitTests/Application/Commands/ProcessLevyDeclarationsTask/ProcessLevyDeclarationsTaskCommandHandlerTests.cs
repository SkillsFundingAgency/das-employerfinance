using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTask;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountBalance;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessLevyDeclarationsTask
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsTaskCommandHandlerTests : FluentTest<ProcessLevyDeclarationsTaskCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingImportLevyDeclarationsCommand_ThenShouldCreateJobTask()
        {
            return TestAsync(f => f.HandleImportLevyDeclarationsCommand(), f => f.Db.ProcessLevyDeclarationsJobTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<ProcessLevyDeclarationsJobTask>(t =>
                    t.JobId == f.ImportLevyDeclarationsCommand.JobId &&
                    t.Type == ProcessLevyDeclarationsJobTaskType.ImportLevyDeclarations &&
                    t.AccountPayeSchemeId == f.ImportLevyDeclarationsCommand.AccountPayeSchemeId &&
                    t.Started >= f.Now &&
                    t.Finished >= f.NextTaskInvoked));
        }
        
        [Test]
        public Task Handle_WhenHandlingImportLevyDeclarationsCommand_ThenShouldInvokeNextTask()
        {
            return TestAsync(f => f.HandleImportLevyDeclarationsCommand(), f => f.NextTask.Verify(c => c(), Times.Once()));
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdateAccountBalanceCommand_ThenShouldCreateJobTask()
        {
            return TestAsync(f => f.HandleUpdateAccountBalanceCommand(), f => f.Db.ProcessLevyDeclarationsJobTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<ProcessLevyDeclarationsJobTask>(t =>
                    t.JobId == f.ImportLevyDeclarationsCommand.JobId &&
                    t.Type == ProcessLevyDeclarationsJobTaskType.UpdateAccountBalance &&
                    t.AccountPayeSchemeId == null &&
                    t.AccountId == f.UpdateAccountBalanceCommand.AccountId &&
                    t.Started >= f.Now &&
                    t.Finished >= f.NextTaskInvoked));
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdateAccountBalanceCommand_ThenShouldInvokeNextTask()
        {
            return TestAsync(f => f.HandleUpdateAccountBalanceCommand(), f => f.NextTask.Verify(c => c(), Times.Once()));
        }
    }

    public class ProcessLevyDeclarationsTaskCommandHandlerTestsFixture
    {
        public DateTime Now { get; set; }
        public Guid JobId { get; set; }
        public long AccountPayeSchemeId { get; set; }
        public long AccountId { get; set; }
        public ImportLevyDeclarationsCommand ImportLevyDeclarationsCommand { get; set; }
        public UpdateAccountBalanceCommand UpdateAccountBalanceCommand { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public ProcessLevyDeclarationsTaskCommandHandler Handler { get; set; }
        public Mock<RequestHandlerDelegate<Unit>> NextTask { get; set; }
        public DateTime NextTaskInvoked { get; set; }

        public ProcessLevyDeclarationsTaskCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            AccountPayeSchemeId = 1;
            AccountId = 2;
            ImportLevyDeclarationsCommand = new ImportLevyDeclarationsCommand(JobId, Now, AccountPayeSchemeId);
            UpdateAccountBalanceCommand = new UpdateAccountBalanceCommand(JobId, AccountId);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            Handler = new ProcessLevyDeclarationsTaskCommandHandler(Db);
            NextTask = new Mock<RequestHandlerDelegate<Unit>>();
            
            NextTask.Setup(t => t()).Callback(() => NextTaskInvoked = DateTime.UtcNow).ReturnsAsync(Unit.Value);
        }

        public async Task HandleImportLevyDeclarationsCommand()
        {
            await Handler.Handle(ImportLevyDeclarationsCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }

        public async Task HandleUpdateAccountBalanceCommand()
        {
            await Handler.Handle(UpdateAccountBalanceCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }
    }
}