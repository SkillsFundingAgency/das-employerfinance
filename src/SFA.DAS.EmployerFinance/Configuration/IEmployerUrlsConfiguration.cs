namespace SFA.DAS.EmployerFinance.Configuration
{
    public interface IEmployerUrlsConfiguration
    {
        //todo: do we need to prefix with Employer, when it's in the name of the interface?
        string EmployerAccountsBaseUrl { get; }
        string EmployerCommitmentsBaseUrl { get; }
        string EmployerFinanceBaseUrl { get; }
        string EmployerPortalBaseUrl { get; }
        string EmployerProjectionsBaseUrl { get; }
        string EmployerRecruitBaseUrl { get; }
        string EmployerUsersBaseUrl { get; }
    }
}