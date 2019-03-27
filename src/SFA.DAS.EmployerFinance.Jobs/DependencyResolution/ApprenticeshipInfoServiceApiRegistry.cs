using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Types.Configuration;
using SFA.DAS.Providers.Api.Client;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.DependencyResolution
{
    public class ApprenticeshipInfoServiceApiRegistry : Registry
    {
        public ApprenticeshipInfoServiceApiRegistry()
        {
            For<IProviderApiClient>().Use(c => new ProviderApiClient(c.GetInstance<IConfiguration>().GetSection(ConfigurationKeys.ApprenticeshipInfoService).Get<ApprenticeshipInfoServiceApiConfiguration>().BaseUrl));
        }
    }
}