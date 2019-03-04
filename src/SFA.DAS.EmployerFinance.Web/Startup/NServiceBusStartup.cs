using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NServiceBus;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.NServiceBus;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.EmployerFinance.Web.HealthChecks;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using SFA.DAS.UnitOfWork.NServiceBus;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class NServiceBusStartup
    {
        public static IServiceCollection AddWebHealthChecks(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddHealthChecks()
                .AddSqlServer(databaseConnectionString)
                .AddCheck<ApiHealthCheck>(
                    "Employer Finance Api",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] {"ready"})
                .AddCheck<NServiceBusHealthCheck>(
                    "Employer Finance NServiceBus",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] {"ready"});
            
            return services;
        }

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
                    
                    var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EmployerFinanceV2.Web")
                        .UseAzureServiceBusTransport(() => configurationSection.ServiceBusConnectionString, isDevelopment)
                        .UseErrorQueue()
                        .UseInstallers()
                        .UseLicense(configurationSection.NServiceBusLicense)
                        .UseMessageConventions()
                        .UseNewtonsoftJsonSerializer()
                        .UseNLogFactory()
                        .UseOutbox()
                        .UseSqlServerPersistence(() => container.GetInstance<DbConnection>())
                        .UseStructureMapBuilder(container)
                        .UseUnitOfWork();
                    
                    endpointConfiguration.EnableCallbacks(true);
                    
                    //TODO: DO we want to make the discriminator unique?
                    endpointConfiguration.MakeInstanceUniquelyAddressable("SFA.DAS.EmployerFinanceV2.Web");
                    
                    var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
                    
                    return endpoint;
                })
                .AddSingleton<IMessageSession>(s => s.GetService<IEndpointInstance>())
                .AddHostedService<NServiceBusHostedService>();
        }
    }
}