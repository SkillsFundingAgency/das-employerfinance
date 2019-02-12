using SFA.DAS.EmployerFinance.Extensions;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public class EmployerFinanceConfiguration
    {
        public OidcConfiguration Oidc { get; set; }
        public EmployerUrlsConfiguration EmployerUrls { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string ServiceBusConnectionString { get; set; }

        public string NServiceBusLicense
        {
            get => _decodedNServiceBusLicense ?? (_decodedNServiceBusLicense = _nServiceBusLicense.HtmlDecode());
            set => _nServiceBusLicense = value;
        }

        private string _nServiceBusLicense;
        private string _decodedNServiceBusLicense;
    }
}