using System;

namespace SFA.DAS.EmployerFinance.Messages.Events
{
    public class HealthCheckEvent
    {
        public int Id { get; }
        public DateTime Created { get;}

        public HealthCheckEvent(int id, DateTime created)
        {
            Id = id;
            Created = created;
        }
    }
}