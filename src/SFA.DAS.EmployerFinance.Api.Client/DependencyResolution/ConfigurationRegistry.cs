using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Api.Client.Configuration;
using SFA.DAS.EmployerFinance.Api.Client.Configuration.Extensions;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    internal class ConfigurationRegistry : Registry
    {
        //todo: snupkg
        public ConfigurationRegistry()
        {
            For<EmployerFinanceApiClientConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceApiClientSection<EmployerFinanceApiClientConfiguration>()).Singleton();
        }
    }
}