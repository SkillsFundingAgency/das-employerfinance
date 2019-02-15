using Microsoft.Extensions.Hosting;
using SFA.DAS.Authorization;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.Authorization;
using StructureMap;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<IAuthorizationContextProvider>().Use<AuthorizationContextProvider>();
            For<IAuthorizationHandler>().DecorateAllWith<LocalAuthorizationHandler>();
            For<IEmployerFinanceDbContextFactory>().Use<EmployerFinanceDbContextWithNServiceBusTransactionFactory>();
            For<IEmployerUrls>().Use<EmployerUrls>();
            For<IHostingEnvironment>().Use<HostingEnvironmentAdapter>();
        }
    }
}