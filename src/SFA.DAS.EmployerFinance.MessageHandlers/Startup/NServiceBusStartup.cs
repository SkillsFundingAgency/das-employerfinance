using System.Data.Common;
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
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.EmployerFinance.MessageHandlers.Startup
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
                    var employerFinanceConfiguration = configuration.GetEmployerFinanceSection<EmployerFinanceConfiguration>();
                    var isDevelopment = hostingEnvironment.IsDevelopment();
                
                    var endpointConfiguration = new EndpointConfiguration(EndpointName.EmployerFinanceV2MessageHandlers)
                        .UseAzureServiceBusTransport(isDevelopment, () => employerFinanceConfiguration.ServiceBusConnectionString)
                        .UseInstallers()
                        .UseLicense(employerFinanceConfiguration.NServiceBusLicense)
                        .UseMessageConventions()
                        .UseNewtonsoftJsonSerializer()
                        .UseNLogFactory()
                        .UseOutbox()
                        .UseSqlServerPersistence(() => container.GetInstance<DbConnection>())
                        .UseInstallers()
                        .UseStructureMapBuilder(container)
                        .UseUnitOfWork();
                    
                    var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
                    
                    return endpoint;
                })
                .AddHostedService<NServiceBusHostedService>();
        }
    }
}