using Microsoft.Extensions.Hosting;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Startup
{
    public static class StructureMapStartup
    {
        public static IHostBuilder UseStructureMap(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new StructureMapServiceProviderFactory(null));
        }
    }
}