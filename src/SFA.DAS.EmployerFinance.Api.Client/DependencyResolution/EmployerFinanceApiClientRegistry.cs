using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Client.DependencyResolution
{
    public class EmployerFinanceApiClientRegistry : Registry
    {
        public EmployerFinanceApiClientRegistry()
        {
            IncludeRegistry<ConfigurationRegistry>();
            IncludeRegistry<HttpRegistry>();
        }
    }
}