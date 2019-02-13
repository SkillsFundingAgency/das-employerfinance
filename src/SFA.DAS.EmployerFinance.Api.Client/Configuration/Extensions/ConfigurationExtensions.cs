using System.Linq;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Api.Client.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TConfig GetEmployerFinanceApiClientSection<TConfig>(this IConfiguration configuration, params string[] subSectionPath)
        {
            return configuration.GetSection(string.Join(":", Enumerable.Repeat(EmployerFinanceConfigurationKeys.ApiClient, 1).Concat(subSectionPath))).Get<TConfig>();
        }
    }
}