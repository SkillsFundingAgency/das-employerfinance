using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public static class ConfigurationExtensions
    {
        public static TConfig GetEmployerFinanceSection<TConfig>(this IConfiguration configuration, params string[] subSectionPath)
        {
            return configuration.GetSection(string.Join(":", Enumerable.Repeat(ConfigurationKeys.EmployerFinance, 1).Concat(subSectionPath))).Get<TConfig>();
        }
    }
}