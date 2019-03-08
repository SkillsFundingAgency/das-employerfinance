using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Data.Configurations;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data
{
    public class EmployerFinanceDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountPayeScheme> AccountPayeSchemes { get; set; }
        public DbSet<LevyDeclarationSaga> LevyDeclarationSagas { get; set; }
        public DbSet<LevyDeclarationSagaTask> LevyDeclarationSagaTasks { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<User> Users { get; set; }

        public EmployerFinanceDbContext(DbContextOptions<EmployerFinanceDbContext> options) : base(options)
        {
        }

        protected EmployerFinanceDbContext()
        {
        }

        public virtual Task ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountPayeSchemeConfiguration());
            modelBuilder.ApplyConfiguration(new LevyDeclarationSagaConfiguration());
            modelBuilder.ApplyConfiguration(new LevyDeclarationSagaTaskConfiguration());
            modelBuilder.ApplyConfiguration(new ProviderConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}