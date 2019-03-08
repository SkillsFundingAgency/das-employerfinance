using System;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Messages.Commands;
using SFA.DAS.NServiceBus.AzureServiceBus;

namespace SFA.DAS.EmployerFinance.Extensions
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseAzureServiceBusTransport(this EndpointConfiguration config, bool isDevelopment, Func<string> connectionStringBuilder)
        {
            config.UseAzureServiceBusTransport(isDevelopment, connectionStringBuilder, r =>
            {
                r.RouteToEndpoint(typeof(ProcessLevyDeclarationsCommand), EndpointName.EmployerFinanceV2MessageHandlers);
                r.RouteToEndpoint(typeof(RunHealthCheckCommand), EndpointName.EmployerFinanceV2MessageHandlers);
            });

            return config;
        }
    }
}
