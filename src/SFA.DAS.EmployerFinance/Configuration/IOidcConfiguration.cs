namespace SFA.DAS.EmployerFinance.Configuration
{
    public interface IOidcConfiguration
    {
        string Authority { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string MetadataAddress { get; set; }
        string LogoutEndpoint { get; set; }
    }
}