using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    internal class HttpRegistry : Registry
    {
        public HttpRegistry()
        {
            For<IEmployerFinanceApiClient>().Use(c => c.GetInstance<IEmployerFinanceApiClientFactory>().CreateClient()).Singleton();
            For<IEmployerFinanceApiClientFactory>().Use<EmployerFinanceApiClientFactory>();
        }
    }
}