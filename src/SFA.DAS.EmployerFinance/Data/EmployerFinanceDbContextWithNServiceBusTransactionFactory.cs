using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using NServiceBus.Persistence;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.UnitOfWork;

namespace SFA.DAS.EmployerFinance.Data
{
    public class EmployerFinanceDbContextWithNServiceBusTransactionFactory : IEmployerFinanceDbContextFactory
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IUnitOfWorkContext _unitOfWorkContext;
        private readonly ILoggerFactory _loggerFactory;

        public EmployerFinanceDbContextWithNServiceBusTransactionFactory(IHostingEnvironment hostingEnvironment, IUnitOfWorkContext unitOfWorkContext, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _unitOfWorkContext = unitOfWorkContext;
            _loggerFactory = loggerFactory;
        }

        public EmployerFinanceDbContext CreateDbContext()
        {
            var synchronizedStorageSession = _unitOfWorkContext.Find<SynchronizedStorageSession>();
            var sqlStorageSession = synchronizedStorageSession.GetSqlStorageSession();
            
            var optionsBuilder = new DbContextOptionsBuilder<EmployerFinanceDbContext>()
                .UseSqlServer(sqlStorageSession.Connection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (_hostingEnvironment.IsDevelopment())
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
            
            var dbContext = new EmployerFinanceDbContext(optionsBuilder.Options);
            
            dbContext.Database.UseTransaction(sqlStorageSession.Transaction);

            return dbContext;
        }
    }
}