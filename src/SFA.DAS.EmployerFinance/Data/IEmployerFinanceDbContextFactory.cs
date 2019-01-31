namespace SFA.DAS.EmployerFinance.Data
{
    public interface IEmployerFinanceDbContextFactory
    {
        EmployerFinanceDbContext CreateDbContext();
    }
}