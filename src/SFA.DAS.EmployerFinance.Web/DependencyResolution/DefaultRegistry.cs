using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Data;
using StructureMap;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public class DefaultRegistry : Registry
    {
        public DefaultRegistry()
        {
            For<IEmployerFinanceDbContextFactory>().Use<EmployerFinanceDbContextWithNServiceBusTransactionFactory>();
            For<IEmployerUrls>().Use<EmployerUrls>();
            For<IHostingEnvironment>().Use<HostingEnvironmentAdapter>();
        }
    }
}