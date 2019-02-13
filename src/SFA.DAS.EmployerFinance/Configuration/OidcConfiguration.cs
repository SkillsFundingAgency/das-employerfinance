namespace SFA.DAS.EmployerFinance.Configuration
{
    public class OidcConfiguration : IOidcConfiguration
    {
        public string Authority { get; set; }
        public string MetadataAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string LogoutEndpoint { get; set; }
    }
}