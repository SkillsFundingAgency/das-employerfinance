namespace SFA.DAS.EmployerFinance.Configuration
{
    public interface IOidcConfiguration
    {
        string Authority { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string MetaDataAddress { get; set; }
    }
}