using System.Net.Http;

namespace SFA.DAS.EmployerFinance.Api.Client.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}