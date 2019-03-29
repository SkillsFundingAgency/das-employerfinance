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
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.UnitOfWork;
using StructureMap;

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
                    t.Finished >= f.NextTaskInvoked &&
                    t.Errored == null &&
                    t.ErrorMessage == null));
        }
        
        [Test]
        public Task Handle_WhenHandlingImportPayeSchemeLevyDeclarationsCommandAndNextTaskThrowsException_ThenShouldCreateErroredSagaTask()
        {
            return TestAsync(f => f.SetNextTaskException(), f => f.HandleImportPayeSchemeLevyDeclarationsCommandAndSwallowException(), f => f.ScopedDb.LevyDeclarationSagaTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSagaTask>(t =>
                    t.SagaId == f.ImportPayeSchemeLevyDeclarationsCommand.SagaId &&
                    t.Type == LevyDeclarationSagaTaskType.ImportPayeSchemeLevyDeclarations &&
                    t.AccountPayeSchemeId == f.ImportPayeSchemeLevyDeclarationsCommand.AccountPayeSchemeId &&
                    t.AccountId == null &&
                    t.Started >= f.Now &&
                    t.Finished == null &
                    t.Errored >= f.NextTaskInvoked &&
                    t.ErrorMessage == f.NextTaskException.GetAggregateMessage()));
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
                    t.Finished >= f.NextTaskInvoked &&
                    t.Errored == null &&
                    t.ErrorMessage == null));
        }
        
        [Test]
        public Task Handle_WhenHandlingUpdateAccountLevyDeclarationTransactionBalancesCommandAndNextTaskThrowsException_ThenShouldCreateErroredSagaTask()
        {
            return TestAsync(f => f.SetNextTaskException(), f => f.HandleUpdateAccountLevyDeclarationTransactionBalancesCommandAndSwallowException(), f => f.ScopedDb.LevyDeclarationSagaTasks.SingleOrDefault().Should().NotBeNull()
                .And.Match<LevyDeclarationSagaTask>(t =>
                    t.SagaId == f.ImportPayeSchemeLevyDeclarationsCommand.SagaId &&
                    t.Type == LevyDeclarationSagaTaskType.UpdateAccountTransactionBalances &&
                    t.AccountPayeSchemeId == null &&
                    t.AccountId == f.UpdateAccountLevyDeclarationTransactionBalancesCommand.AccountId &&
                    t.Started >= f.Now &&
                    t.Finished == null &&
                    t.Errored >= f.NextTaskInvoked &&
                    t.ErrorMessage == f.NextTaskException.GetAggregateMessage()));
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
        public Mock<IUnitOfWorkScope> UnitOfWorkScope { get; set; }
        public EmployerFinanceDbContext ScopedDb { get; set; }
        public Mock<IContainer> ScopedContainer { get; set; }
        public RunLevyDeclarationSagaTaskCommandHandler Handler { get; set; }
        public Mock<RequestHandlerDelegate<Unit>> NextTask { get; set; }
        public DateTime NextTaskInvoked { get; set; }

        public Exception NextTaskException { get; set; }

        public RunProcessLevyDeclarationsSagaTaskCommandHandlerTestsFixture()
        {
            Now = DateTime.UtcNow;
            AccountPayeSchemeId = 1;
            AccountId = 2;
            ImportPayeSchemeLevyDeclarationsCommand = new ImportPayeSchemeLevyDeclarationsCommand(SagaId, Now, AccountPayeSchemeId);
            UpdateAccountLevyDeclarationTransactionBalancesCommand = new UpdateAccountLevyDeclarationTransactionBalancesCommand(SagaId, AccountId);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UnitOfWorkScope = new Mock<IUnitOfWorkScope>();
            ScopedDb = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            ScopedContainer = new Mock<IContainer>();
            Handler = new RunLevyDeclarationSagaTaskCommandHandler(Db, UnitOfWorkScope.Object);
            NextTask = new Mock<RequestHandlerDelegate<Unit>>();

            UnitOfWorkScope.Setup(s => s.RunAsync(It.IsAny<Func<IContainer, Task>>())).Returns<Func<IContainer, Task>>(o => o(ScopedContainer.Object));
            ScopedContainer.Setup(c => c.GetInstance<EmployerFinanceDbContext>()).Returns(ScopedDb);
            NextTask.Setup(t => t()).Callback(() => NextTaskInvoked = DateTime.UtcNow).ReturnsAsync(Unit.Value);
        }

        public async Task HandleImportPayeSchemeLevyDeclarationsCommand()
        {
            await Handler.Handle(ImportPayeSchemeLevyDeclarationsCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }

        public async Task HandleImportPayeSchemeLevyDeclarationsCommandAndSwallowException()
        {
            try
            {
                await HandleImportPayeSchemeLevyDeclarationsCommand();
            }
            catch (Exception)
            {
            }
        }

        public async Task HandleUpdateAccountLevyDeclarationTransactionBalancesCommand()
        {
            await Handler.Handle(UpdateAccountLevyDeclarationTransactionBalancesCommand, CancellationToken.None, NextTask.Object);
            await Db.SaveChangesAsync();
        }

        public async Task HandleUpdateAccountLevyDeclarationTransactionBalancesCommandAndSwallowException()
        {
            try
            {
                await HandleUpdateAccountLevyDeclarationTransactionBalancesCommand();
            }
            catch (Exception)
            {
            }
        }

        public RunProcessLevyDeclarationsSagaTaskCommandHandlerTestsFixture SetNextTaskException()
        {
            NextTaskException = new Exception("Foo", new Exception("Bar"));

            NextTask.Setup(t => t()).ThrowsAsync(NextTaskException);
            
            return this;
        }
    }
}