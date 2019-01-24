using NServiceBus;

namespace SFA.DAS.EmployerFinance.Messages
{
    public class DummyEvent : IEvent
    {
        public string Payload { get; set; }
    }
}