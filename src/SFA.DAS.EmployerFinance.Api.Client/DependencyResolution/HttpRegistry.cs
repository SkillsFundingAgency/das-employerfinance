using System.Net.Http;
using SFA.DAS.EmployerFinance.Api.Client.Http;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    internal class HttpRegistry : Registry
    {
        public HttpRegistry()
        {
            For<HttpClient>().Add(c => c.GetInstance<IHttpClientFactory>().CreateHttpClient()).Named(GetType().FullName).Singleton();
            For<IHttpClientFactory>().Use<Http.HttpClientFactory>();
            For<IRestHttpClient>().Use<RestHttpClient>().Ctor<HttpClient>().IsNamedInstance(GetType().FullName);
            For<IEmployerFinanceApiClient>().Use<EmployerFinanceApiClient>();
        }
    }
}