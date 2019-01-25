namespace SFA.DAS.EmployerFinance.Configuration
{
    public interface IOidcConfiguration
    {
        string Authority { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string MetaDataAddress { get; set; }

//        string AccountActivationUrl { get; }
//        string AuthorizeEndpoint { get; }
//        string BaseAddress { get; }
//        string ChangeEmailUrl { get; }
//        string ChangePasswordUrl { get; }
//        string LogoutEndpoint { get; }
//        string Scopes { get; }
//        string TokenCertificateThumbprint { get; }
//        string TokenEndpoint { get; }
//        bool UseCertificate { get; }
//        string UserInfoEndpoint { get; }
    }
}