using SFA.DAS.Http.Configuration;

namespace SFA.DAS.EmployerFinance.Api.Client.Configuration
{
    public class EmployerFinanceApiClientConfiguration : IAzureActiveDirectoryClientConfiguration
    {
        public string ApiBaseUrl { get; set; }
        public string Tenant { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string IdentifierUri { get; set; }
    }
}