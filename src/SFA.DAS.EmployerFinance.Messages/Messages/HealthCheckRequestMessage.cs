using System;
using NServiceBus;

namespace SFA.DAS.EmployerFinance.Messages.Messages
{
    public class HealthCheckRequestMessage : IMessage
    {
        public Guid Id { get; set; }
    }
}