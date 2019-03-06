using System;
using NServiceBus;
using SFA.DAS.EmployerFinance.Messages.Commands;
using SFA.DAS.NServiceBus.AzureServiceBus;

namespace SFA.DAS.EmployerFinance.NServiceBus
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseAzureServiceBusTransport(this EndpointConfiguration config, Func<string> connectionStringBuilder, bool isDevelopment)
        {
            config.UseAzureServiceBusTransport(isDevelopment, connectionStringBuilder, r =>
            {
                r.RouteToEndpoint(typeof(RunHealthCheckCommand).Assembly, "SFA.DAS.EmployerFinanceV2.MessageHandlers");
            });

            return config;
        }
    }
}
