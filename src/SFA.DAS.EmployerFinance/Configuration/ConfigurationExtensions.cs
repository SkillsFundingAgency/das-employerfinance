using System.Linq;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public static class ConfigurationExtensions
    {
        public static TConfig GetEmployerFinanceSection<TConfig>(this IConfiguration configuration, params string[] subSectionPath)
        {
            return configuration.GetSection(string.Join(":", Enumerable.Repeat(EmployerFinanceConfigurationKeys.Base, 1).Concat(subSectionPath))).Get<TConfig>();
        }
    }
}