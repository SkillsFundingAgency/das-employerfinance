using SFA.DAS.EmployerFinance.Api.Client.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerFinance.Api.Client
{
    public class EmployerFinanceApiClientFactory : IEmployerFinanceApiClientFactory
    {
        private readonly EmployerFinanceApiClientConfiguration _configuration;

        public EmployerFinanceApiClientFactory(EmployerFinanceApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public IEmployerFinanceApiClient CreateClient()
        {
            var httpClientFactory = new AzureActiveDirectoryHttpClientFactory(_configuration);
            var httpClient = httpClientFactory.CreateHttpClient();
            var restHttpClient = new RestHttpClient(httpClient);
            var apiClient = new EmployerFinanceApiClient(restHttpClient);

            return apiClient;
        }
    }
}