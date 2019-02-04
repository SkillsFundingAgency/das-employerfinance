using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NLog.Web;
using StructureMap.AspNetCore;

namespace SFA.DAS.EmployerFinance.Web
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(o => o.AddServerHeader = false)
                .UseNLog()
                .UseStartup<Startup>()
                .UseStructureMap();
    }

}