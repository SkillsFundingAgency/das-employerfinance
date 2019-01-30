using SFA.DAS.AutoConfiguration.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    public class EmployerFinanceApiClientRegistry : Registry
    {
        public EmployerFinanceApiClientRegistry()
        {
            IncludeRegistry<AutoConfigurationRegistry>();
            IncludeRegistry<ConfigurationRegistry>();
            IncludeRegistry<HttpRegistry>();
          
        }
    }
}