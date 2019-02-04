using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.EmployerFinance.Models;

namespace SFA.DAS.EmployerFinance.Data
{
    public class EmployerFinanceDbContext : DbContext
    {
        public DbSet<HealthCheck> HealthChecks { get; set; }

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
            modelBuilder.ApplyConfiguration(new HealthCheckConfiguration());
        }
    }
}