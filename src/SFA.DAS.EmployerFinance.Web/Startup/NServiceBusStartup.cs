using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public class NServiceBusStartup : IRunAtStartup
    {
        private readonly IContainer _container;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly EmployerFinanceConfiguration _employerFinanceConfiguration;
        private IEndpointInstance _endpoint;

        public NServiceBusStartup(IContainer container, IHostingEnvironment hostingEnvironment, EmployerFinanceConfiguration employerFinanceConfiguration)
        {
            _container = container;
            _hostingEnvironment = hostingEnvironment;
            _employerFinanceConfiguration = employerFinanceConfiguration;
        }

        public async Task StartAsync()
        {
            var endpointConfiguration = new EndpointConfiguration("SFA.DAS.EmployerFinanceV2.Web")
                .UseAzureServiceBusTransport(() => _employerFinanceConfiguration.ServiceBusConnectionString, _hostingEnvironment.IsDevelopment())
                .UseErrorQueue()
                .UseInstallers()
                .UseLicense(_employerFinanceConfiguration.NServiceBusLicense)
                .UseMessageConventions()
                .UseSqlServerPersistence(() => _container.GetInstance<DbConnection>())
                .UseNewtonsoftJsonSerializer()
                .UseNLogFactory()
                .UseOutbox()
                .UseStructureMapBuilder(_container)
                .UseUnitOfWork();

            _endpoint = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            _container.Configure(c => c.For<IMessageSession>().Use(_endpoint));
        }

        public Task StopAsync()
        {
            return _endpoint.Stop();
        }
    }
}