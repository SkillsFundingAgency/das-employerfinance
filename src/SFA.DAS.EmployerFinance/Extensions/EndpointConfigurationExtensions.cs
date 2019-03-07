using System;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations;
using SFA.DAS.NServiceBus.AzureServiceBus;

namespace SFA.DAS.EmployerFinance.Extensions
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration UseAzureServiceBusTransport(this EndpointConfiguration config, bool isDevelopment, Func<string> connectionStringBuilder)
        {
            config.UseAzureServiceBusTransport(isDevelopment, connectionStringBuilder, r =>
            {
                r.RouteToEndpoint(typeof(ProcessLevyDeclarationsCommand), "SFA.DAS.EmployerFinanceV2.MessageHandlers");
            });

            return config;
        }
    }
}
