using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using NServiceBus;
using NServiceBus.UniformSession;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.ImportLevyDeclarations
{
    [TestFixture]
    [Parallelizable]
    public class ImportLevyDeclarationsCommandHandlerTests : FluentTest<ImportLevyDeclarationsCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommandAndSagaTypeIsScheduled_ThenShouldSendImportCommands()
        {
            return TestAsync(f => f.Handle(), f =>
            {
                f.UniformSession.Verify(s => s.Send(It.IsAny<ImportPayeSchemeLevyDeclarationsCommand>(), It.IsAny<SendOptions>()), Times.Exactly(f.AccountPayeSchemes.Count));
                
                f.AccountPayeSchemes.ForEach(aps => f.UniformSession.Verify(s => s.Send(
                    It.Is<ImportPayeSchemeLevyDeclarationsCommand>(c =>
                        c.SagaId == f.Command.SagaId &&
                        c.PayrollPeriod == f.Command.PayrollPeriod &&
                        c.AccountPayeSchemeId == aps.Id),
                    It.IsAny<SendOptions>()), Times.Once));
            });
        }
    }

    public class ImportLevyDeclarationsCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public ImportLevyDeclarationsCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<ImportLevyDeclarationsCommand> Handler { get; set; }
        public List<string> EmployerReferenceNumbers { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }
        public AccountPayeScheme ExcludedAccountPayeScheme { get; set; }
        
        public ImportLevyDeclarationsCommandHandlerTestsFixture()
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

            ExcludedAccountPayeScheme = Fixture.Create<AccountPayeScheme>().Set(aps => aps.Id, 4);
            
            Command = new ImportLevyDeclarationsCommand(5, Now, AccountPayeSchemes.Max(aps => aps.Id));
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();
            
            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes.Append(ExcludedAccountPayeScheme));
            Db.SaveChanges();
            
            Handler = new ImportLevyDeclarationsCommandHandler(Db, UniformSession.Object);
        }

        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}