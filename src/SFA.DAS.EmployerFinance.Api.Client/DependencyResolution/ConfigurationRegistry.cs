using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Api.Client.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    internal class ConfigurationRegistry : Registry
    {
        public ConfigurationRegistry()
        {
            For<EmployerFinanceApiClientConfiguration>().Use(c =>
                c.GetInstance<IAutoConfigurationService>()
                    .Get<EmployerFinanceApiClientConfiguration>(ConfigurationKeys.ApiClient)).Singleton();
        }

    }
}