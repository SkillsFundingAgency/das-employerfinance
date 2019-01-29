using System;
using System.Net.Http;
using SFA.DAS.EmployerFinance.Api.Client.Configuration;
using SFA.DAS.Http;

namespace SFA.DAS.EmployerFinance.Api.Client.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly EmployerFinanceApiClientConfiguration _configuration;

        public HttpClientFactory(EmployerFinanceApiClientConfiguration configuration)
        {
            _configuration = configuration;
        }

        public HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClientBuilder()
                .WithDefaultHeaders()
                .Build();
            
            httpClient.BaseAddress = new Uri(_configuration.ApiBaseUrl);

            return httpClient;
        }
    }
}