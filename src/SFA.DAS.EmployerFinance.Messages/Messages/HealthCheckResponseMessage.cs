using System;
using NServiceBus;

namespace SFA.DAS.EmployerFinance.Messages.Messages
{
    public class HealthCheckResponseMessage : IMessage
    {
        public Guid Id { get; set; }
    }
}