using System;
using System.Threading.Tasks;
using NServiceBus;

namespace SFA.DAS.EmployerFinance.Extensions
{
    public static class MessageHandlerContextExtensions
    {
        public static Task SendLocal(this IMessageHandlerContext messageHandlerContext, object message, TimeSpan delay)
        {
            var sendOptions = new SendOptions();

            sendOptions.DelayDeliveryWith(delay);
            sendOptions.RouteToThisEndpoint();

            return messageHandlerContext.Send(message, sendOptions);
        }
    }
}