using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.UniformSession;

namespace SFA.DAS.EmployerFinance.Extensions
{
    public static class UniformSessionExtensions
    {
        public static Task SendLocal(this IUniformSession uniformSession, object message, TimeSpan delay)
        {
            var sendOptions = new SendOptions();

            sendOptions.DelayDeliveryWith(delay);
            sendOptions.RouteToThisEndpoint();

            return uniformSession.Send(message, sendOptions);
        }
    }
}