using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerFinance.Data
{
    public class EmployerFinanceDbContextWithNewTransactionFactory : IEmployerFinanceDbContextFactory
    {
        private readonly DbConnection _dbConnection;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoggerFactory _loggerFactory;

        public EmployerFinanceDbContextWithNewTransactionFactory(DbConnection dbConnection, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _dbConnection = dbConnection;
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public EmployerFinanceDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployerFinanceDbContext>()
                .UseSqlServer(_dbConnection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (_hostingEnvironment.IsDevelopment())
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
            
            var dbContext = new EmployerFinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}