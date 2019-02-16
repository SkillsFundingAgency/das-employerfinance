using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using SFA.DAS.EmployerFinance.Api.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureDasAppConfiguration()
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseKestrel(o => o.AddServerHeader = false)
                .UseStructureMap()
                .UseStartup<AspNetStartup>();
    }
}