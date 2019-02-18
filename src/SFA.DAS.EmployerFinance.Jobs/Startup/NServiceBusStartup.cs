using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.StructureMap;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Jobs.Startup
{
    public static class NServiceBusStartup
    {
        public static IServiceCollection AddDasNServiceBus(this IServiceCollection services)
        {
            return services
                .AddSingleton(s =>
                {
                    var configuration = s.GetService<IConfiguration>();
                    var container = s.GetService<IContainer>();
                    var hostingEnvironment = s.GetService<IHostingEnvironment>();
                    var configurationSection = configuration.GetEmployerFinanceSection<EmployerFinanceConfiguration>();
                    var isDevelopment = hostingEnvironment.IsDevelopment();
                
                    var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EmployerFinanceV2.Jobs")
                        .UseAzureServiceBusTransport(isDevelopment, () => configurationSection.ServiceBusConnectionString)
                        .UseInstallers()
                        .UseLicense(configurationSection.NServiceBusLicense)
                        .UseMessageConventions()
                        .UseNewtonsoftJsonSerializer()
                        .UseNLogFactory()
                        .UseStructureMapBuilder(container)
                        .UseSendOnly();
                    
                    var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
                    
                    return endpoint;
                })
                .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();
        }
    }
}