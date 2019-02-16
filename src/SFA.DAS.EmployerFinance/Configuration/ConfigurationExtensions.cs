using System.Linq;
using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Types.Configuration;

namespace SFA.DAS.EmployerFinance.Configuration
{
    public static class ConfigurationExtensions
    {
        public static TConfiguration GetEmployerFinanceSection<TConfiguration>(this IConfiguration configuration, params string[] subSectionPaths)
        {
            var key = string.Join(":", Enumerable.Repeat(EmployerFinanceConfigurationKeys.Base, 1).Concat(subSectionPaths));
            var configurationSection = configuration.GetSection(key);
            var value = configurationSection.Get<TConfiguration>();

            return value;
        }
    }
}