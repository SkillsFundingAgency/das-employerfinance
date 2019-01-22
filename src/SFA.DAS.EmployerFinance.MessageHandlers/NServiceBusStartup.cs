using System.Data.Common;
using System.Threading.Tasks;
using NServiceBus;
using SFA.DAS.AutoConfiguration;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Extensions;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.NServiceBus;
using SFA.DAS.NServiceBus.NewtonsoftJsonSerializer;
using SFA.DAS.NServiceBus.NLog;
using SFA.DAS.NServiceBus.SqlServer;
using SFA.DAS.NServiceBus.StructureMap;
using StructureMap;


namespace SFA.DAS.EmployerFinance.MessageHandlers
{
    public class NServiceBusStartup : IStartup
    {
        private readonly IContainer _container;
        private readonly IEnvironmentService _environmentService;
        private readonly EmployerFinanceConfiguration _employerFinanceConfiguration;
        private IEndpointInstance _endpoint;

        public NServiceBusStartup(
            IContainer container, 
            IEnvironmentService environmentService,
            EmployerFinanceConfiguration employerFinanceConfiguration)
        {
            _container = container;
            _environmentService = environmentService;
            _employerFinanceConfiguration = employerFinanceConfiguration;
        }
        
        public async Task StartAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.ProviderRelationships.JobsV2")
                .UseAzureServiceBusTransport(() => _employerFinanceConfiguration.ServiceBusConnectionString, _environmentService.IsCurrent(DasEnv.LOCAL))
                .UseLicense(_employerFinanceConfiguration.NServiceBusLicense)
                .UseMessageConventions()
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseSendOnly()
                .UseStructureMapBuilder(_container);

            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}
