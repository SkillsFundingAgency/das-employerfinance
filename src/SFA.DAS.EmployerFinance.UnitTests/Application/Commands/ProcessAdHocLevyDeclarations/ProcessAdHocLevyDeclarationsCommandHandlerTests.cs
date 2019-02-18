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
using SFA.DAS.EmployerFinance.Application.Commands.ProcessAdHocLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ProcessAdHocLevyDeclarations
{
    [TestFixture]
    [Parallelizable]
    public class ProcessAdHocLevyDeclarationsCommandHandlerTests : FluentTest<ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldCreateJob()
        {
            return TestAsync(f => f.Handle(), f => f.Db.ProcessLevyDeclarationsJobs.SingleOrDefault().Should().NotBeNull()
                .And.Match<ProcessLevyDeclarationsJob>(j =>
                    j.PayrollPeriod == f.Command.PayrollPeriod &&
                    j.AccountPayeScheme == f.AccountPayeScheme &&
                    j.ImportLevyDeclarationsTasksCount == 1 &&
                    j.ImportLevyDeclarationsTasksCompletedCount == 0 &&
                    j.UpdateAccountBalanceTasksCount == 1 &&
                    j.UpdateAccountBalanceTasksCompletedCount == 0 &&
                    j.Created >= f.Now &&
                    j.Updated == null &&
                    !j.IsComplete));
        }
        
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendImportCommands()
        {
            return TestAsync(f => f.Handle(), f => f.UniformSession
                .Verify(s => s.Send(
                    It.Is<ImportLevyDeclarationsCommand>(c =>
                        c.JobId == f.Db.ProcessLevyDeclarationsJobs.Select(j => j.Id).Single() &&
                        c.PayrollPeriod == f.Command.PayrollPeriod &&
                        c.AccountPayeSchemeId == f.AccountPayeScheme.Id),
                    It.IsAny<SendOptions>())));
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

    public class ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public ProcessAdHocLevyDeclarationsCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<ProcessAdHocLevyDeclarationsCommand> Handler { get; set; }
        public AccountPayeScheme AccountPayeScheme { get; set; }

        public ProcessAdHocLevyDeclarationsCommandHandlerTestsFixture()
        {
            Fixture = new Fixture();
            Now = DateTime.UtcNow;
            AccountPayeScheme = Fixture.Create<AccountPayeScheme>().Set(aps => aps.Id, 1);
            Command = new ProcessAdHocLevyDeclarationsCommand(Now, AccountPayeScheme.Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();
            
            Db.AccountPayeSchemes.Add(AccountPayeScheme);
            Db.SaveChanges();
            
            Handler = new ProcessAdHocLevyDeclarationsCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}