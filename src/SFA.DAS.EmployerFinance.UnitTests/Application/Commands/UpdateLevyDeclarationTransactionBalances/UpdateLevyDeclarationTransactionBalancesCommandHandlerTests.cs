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
using SFA.DAS.EmployerFinance.Application.Commands.UpdateAccountLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Application.Commands.UpdateLevyDeclarationTransactionBalances;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Models;
using SFA.DAS.Testing;
using SFA.DAS.Testing.Builders;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Commands.UpdateLevyDeclarationTransactionBalances
{
    [TestFixture]
    [Parallelizable]
    public class UpdateLevyDeclarationTransactionBalancesCommandHandlerTests : FluentTest<UpdateLevyDeclarationTransactionBalancesCommandHandlerTestsFixture>
    {
        [Test]
        public Task Handle_WhenHandlingCommand_ThenShouldSendCommands()
        {
            return TestAsync(f => f.Handle(), f =>
            {
                f.UniformSession.Verify(s => s.Send(It.IsAny<UpdateAccountLevyDeclarationTransactionBalancesCommand>(), It.IsAny<SendOptions>()), Times.Exactly(f.Accounts.Count));
                
                f.Accounts.ForEach(a => f.UniformSession.Verify(s => s.Send(
                    It.Is<UpdateAccountLevyDeclarationTransactionBalancesCommand>(c =>
                        c.SagaId == f.Command.SagaId &&
                        c.AccountId == a.Id),
                    It.IsAny<SendOptions>()), Times.Once));
            });
        }
    }

    public class UpdateLevyDeclarationTransactionBalancesCommandHandlerTestsFixture
    {
        public Fixture Fixture { get; set; }
        public DateTime Now { get; set; }
        public UpdateLevyDeclarationTransactionBalancesCommand Command { get; set; }
        public EmployerFinanceDbContext Db { get; set; }
        public Mock<IUniformSession> UniformSession { get; set; }
        public IRequestHandler<UpdateLevyDeclarationTransactionBalancesCommand> Handler { get; set; }
        public List<string> EmployerReferenceNumbers { get; set; }
        public List<Account> Accounts { get; set; }
        public List<AccountPayeScheme> AccountPayeSchemes { get; set; }
        public LevyDeclarationSaga Saga { get; set; }
        public List<LevyDeclarationSagaTask> LevyDeclarationSagaTasks { get; set; }
        
        public UpdateLevyDeclarationTransactionBalancesCommandHandlerTestsFixture()
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
                Fixture.Create<Account>().Set(a => a.Id, 1),
                Fixture.Create<Account>().Set(a => a.Id, 2)
            };
            
            AccountPayeSchemes = new List<AccountPayeScheme>
            {
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[0], Now).Set(aps => aps.Id, 3),
                new AccountPayeScheme(Accounts[0], EmployerReferenceNumbers[1], Now).Set(aps => aps.Id, 4),
                new AccountPayeScheme(Accounts[1], EmployerReferenceNumbers[2], Now).Set(aps => aps.Id, 5)
            };
            
            Saga = ObjectActivator.CreateInstance<LevyDeclarationSaga>().Set(s => s.Id, 6);
            
            LevyDeclarationSagaTasks = AccountPayeSchemes
                .Select(aps => LevyDeclarationSagaTask.CreateImportPayeSchemeLevyDeclarationsTask(Saga.Id, aps.Id))
                .ToList();
            
            Command = new UpdateLevyDeclarationTransactionBalancesCommand(Saga.Id);
            Db = new EmployerFinanceDbContext(new DbContextOptionsBuilder<EmployerFinanceDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
            UniformSession = new Mock<IUniformSession>();

            Db.AccountPayeSchemes.AddRange(AccountPayeSchemes);
            Db.LevyDeclarationSagas.Add(Saga);
            Db.LevyDeclarationSagaTasks.AddRange(LevyDeclarationSagaTasks);
            Db.SaveChanges();
            
            Handler = new UpdateLevyDeclarationTransactionBalancesCommandHandler(Db, UniformSession.Object);
        }


        public async Task Handle()
        {
            await Handler.Handle(Command, CancellationToken.None);
            await Db.SaveChangesAsync();
        }
    }
}