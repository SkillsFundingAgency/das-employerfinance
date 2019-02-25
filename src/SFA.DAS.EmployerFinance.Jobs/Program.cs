using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.EmployerFinance.Jobs.DependencyResolution;
using SFA.DAS.EmployerFinance.Jobs.ScheduledJobs;
using SFA.DAS.EmployerFinance.Jobs.Startup;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //todo: according to https://docs.microsoft.com/en-us/azure/app-service/webjobs-sdk-get-started
        // The Azure Storage emulator that runs locally doesn't have all of the features that the WebJobs SDK needs
        // is that true?? needs blob storage for singleton locks, logs??
        
        //note: updating Microsoft.Azure.WebJobs to 3.0.4 & Microsoft.Azure.WebJobs.Extensions.Storage to 3.0.3 breaks this (at least locally)!
        //https://stackoverflow.com/questions/54510540/azure-function-app-failing-to-load-ioptionsformatter
        
        //https://dotnetcoretutorials.com/2018/12/05/azure-webjobs-in-net-core-part-5/
        
        //need to install latest azure-functions-core-tools????
        //https://social.msdn.microsoft.com/Forums/azure/en-US/ed2954e5-04ee-47e7-bbc6-3cf747d5c30d/no-job-functions-found-try-making-your-job-classes-and-methods-public-if-youre-using-binding?forum=AzureFunctions
        
        //https://docs.microsoft.com/en-us/azure/app-service/webjobs-sdk-get-started
        
        private static IHostBuilder CreateHostBuilder(string[] args) => 
            new HostBuilder()
                .ConfigureDasWebJobs()
                .ConfigureDasAppConfiguration(args)
                .ConfigureDasLogging()
                .UseDasEnvironment()
                .UseStructureMap()
//                .ConfigureServices((hostContext, services) => { services.Configure<ConsoleLifetimeOptions>(o =>
//                {
//                    o.SuppressStatusMessages = false; }); })
                .UseConsoleLifetime()
                .ConfigureServices(s => s.AddDasNServiceBus())
                //.ConfigureServices(s => s.AddSingleton<ImportProvidersJob>())
                .ConfigureServices(s => s.AddTransient<ImportProvidersJob>())
                .ConfigureContainer<Registry>(IoC.Initialize);
    }
}