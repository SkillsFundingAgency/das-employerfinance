using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerFinance.Web.Startup;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureDasAppConfiguration()
                .ConfigureDasLogging()
                .UseStructureMap()
                .UseStartup<AspNetCoreStartup>();
        }
    }
}