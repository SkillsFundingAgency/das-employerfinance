using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
    public class NServiceBusStartup : IRunAtStartup
    {
        private readonly IContainer _container;
        private readonly IHostingEnvironment _environment;
        private readonly EmployerFinanceConfiguration _employerFinanceConfiguration;
        private IEndpointInstance _endpoint;

        public NServiceBusStartup(
            IContainer container,
            IHostingEnvironment environment,
            EmployerFinanceConfiguration employerFinanceConfiguration)
        {
            _container = container;
            _environment = environment;
            _employerFinanceConfiguration = employerFinanceConfiguration;
        }
        
        public async Task StartAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EmployerFinanceV2.Jobs")
                .UseAzureServiceBusTransport(() => _employerFinanceConfiguration.ServiceBusConnectionString, _environment.IsDevelopment())
                .UseInstallers()
                .UseLicense(_employerFinanceConfiguration.NServiceBusLicense)
                .UseMessageConventions()
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseStructureMapBuilder(_container)
                .UseSendOnly();

            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}
