using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.Api.Client.Http;

namespace SFA.DAS.EmployerFinance.Api.Client
{
    public class EmployerFinanceApiClient : IEmployerFinanceApiClient
    {
        private readonly IRestHttpClient _restHttpClient;

        public EmployerFinanceApiClient(IRestHttpClient restHttpClient)
        {
            _restHttpClient = restHttpClient;
        }
        
        public Task<string> Ping()
        {
            return _restHttpClient.Get("ping");
        }
    }
}