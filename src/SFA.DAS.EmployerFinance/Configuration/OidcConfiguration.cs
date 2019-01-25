namespace SFA.DAS.EmployerFinance.Configuration
{
    public class OidcConfiguration : IOidcConfiguration
    {
        public string Authority { get; set; }
        public string MetaDataAddress { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}