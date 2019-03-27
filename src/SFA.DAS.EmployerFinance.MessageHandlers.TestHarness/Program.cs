using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.DependencyResolution;
using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Scenarios;
using SFA.DAS.EmployerFinance.MessageHandlers.TestHarness.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers.TestHarness
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.StartAsync();

            var publishAddedPayeSchemeEvent = host.Services.GetService<PublishEmployerAccountsEvents>();
            await publishAddedPayeSchemeEvent.Run();

            await host.StopAsync();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args) => 
            new HostBuilder()
                .ConfigureDasAppConfiguration(args)
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseStructureMap()
                .ConfigureServices(s => s.AddDasNServiceBus())
                .ConfigureContainer<Registry>(IoC.Initialize);
    }
}