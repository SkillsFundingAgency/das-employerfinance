using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NServiceBus;
using NServiceBus.UniformSession;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessLevyDeclarations
{
    [TestFixture]
    [Parallelizable]
    public class ProcessLevyDeclarationsCommandHandlerTests : FluentTest<ProcessLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldCreateJob()
        {
            return TestAsync(f => f.Handle(), f => f.Db.ProcessLevyDeclarationsJobs.SingleOrDefault().Should().NotBeNull()
                .And.Match<ProcessLevyDeclarationsJob>(j =>
                    j.PayrollPeriod == f.Command.PayrollPeriod &&
                    j.AccountPayeScheme == null &&
                    j.AccountPayeSchemeId == null &&
                    j.ImportLevyDeclarationsTasksCount == f.EmployerReferenceNumbers.Count &&
                    j.ImportLevyDeclarationsTasksCompletedCount == 0 &&
                    j.UpdateAccountBalanceTasksCount == f.Accounts.Count &&
                    j.UpdateAccountBalanceTasksCompletedCount == 0 &&
                    j.Created >= f.Now &&
                    j.Updated == null &&
                    !j.IsComplete));
        }
        
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendImportCommands()
        {
            return TestAsync(f => f.Handle(), f => f.AccountPayeSchemes.ForEach(aps => f.UniformSession
                .Verify(s => s.Send(
                    It.Is<ImportLevyDeclarationsCommand>(c =>
                        c.JobId == f.Db.ProcessLevyDeclarationsJobs.Select(j => j.Id).Single() &&
                        c.PayrollPeriod == f.Command.PayrollPeriod &&
                        c.AccountPayeSchemeId == aps.Id),
                    It.IsAny<SendOptions>()))));
        }
        
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendProcessTimeoutCommand()
        {
            return TestAsync(f => f.Handle(), f => f.UniformSession
                .Verify(s => s.Send(
                    It.Is<ProcessLevyDeclarationsTimeoutCommand>(c =>
                        c.JobId == f.Db.ProcessLevyDeclarationsJobs.Select(j => j.Id).Single()),
                    It.IsAny<SendOptions>())));
        }
    }

    public class ProcessLevyDeclarationsCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public ProcessLevyDeclarationsCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<ProcessLevyDeclarationsCommand> Handler { get; set; }
        public List<string> EmployerReferenceNumbers { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }

        public ProcessLevyDeclarationsCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            
            EmployerReferenceNumbers = new List<string>
            {
                "AAA111",
                "BBB222",
                "CCC333"
            };
            
            Accounts = new List<Account>
            {
                Fixture.Create<Account>(),
                Fixture.Create<Account>()
            };
            
            AccountPayeSchemes = new List<AccountPayeScheme>
            {
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[0]).Set(aps => aps.Id, 1),
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[1]).Set(aps => aps.Id, 2),
                new AccountPayeScheme(Accounts[1], EmployerReferenceNumbers[2]).Set(aps => aps.Id, 3)
            };
            
            Command = new ProcessLevyDeclarationsCommand(Now);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();
            
            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.SaveChanges();
            
            Handler = new ProcessLevyDeclarationsCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}