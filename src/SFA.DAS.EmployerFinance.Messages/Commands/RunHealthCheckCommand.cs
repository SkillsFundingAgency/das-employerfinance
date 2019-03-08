using System;

namespace SFA.DAS.EmployerFinance.Messages.Commands
{
    public class RunHealthCheckCommand
    {
        public Guid Id { get; }

        public RunHealthCheckCommand(Guid id)
        {
            Id = id;
        }
    }
}