namespace SFA.DAS.EmployerFinance.Api.Client
{
    public interface IEmployerFinanceApiClientFactory
    {
        IEmployerFinanceApiClient CreateClient();
    }
}