using Microsoft.Extensions.Configuration;
using SFA.DAS.Configuration.AzureTableStorage;
using SFA.DAS.EmployerFinance.Api.Client.Configuration;
using SFA.DAS.EmployerFinance.Types.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    internal class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<EmployerFinanceApiClientConfiguration>().Use(c => c.GetInstance<IConfiguration>().GetEmployerFinanceApiClientSection<EmployerFinanceApiClientConfiguration>()).Singleton();
        }
    }
}