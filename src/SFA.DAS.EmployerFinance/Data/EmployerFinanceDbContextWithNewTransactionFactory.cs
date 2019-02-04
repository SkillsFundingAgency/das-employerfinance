using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SFA.DAS.AutoConfiguration;

namespace SFA.DAS.EmployerFinance.Data
{
    public class EmployerFinanceDbContextWithNewTransactionFactory : IEmployerFinanceDbContextFactory
    {
        private readonly DbConnection _dbConnection;
        private readonly IEnvironmentService _environmentService;
        private readonly ILoggerFactory _loggerFactory;

        public EmployerFinanceDbContextWithNewTransactionFactory(DbConnection dbConnection, IEnvironmentService environmentService, ILoggerFactory loggerFactory)
        {
            _dbConnection = dbConnection;
            _environmentService = environmentService;
            _loggerFactory = loggerFactory;
        }

        public EmployerFinanceDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EmployerFinanceDbContext>()
                .UseSqlServer(_dbConnection)
                .ConfigureWarnings(w => w.Throw(RelationalEventId.QueryClientEvaluationWarning));

            if (_environmentService.IsCurrent(DasEnv.LOCAL))
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
            }
            
            var dbContext = new EmployerFinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}