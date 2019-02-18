using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsTimeout
{
    public class ProcessLevyDeclarationsTimeoutCommand : IRequest
    {
        public Guid JobId { get; }

        public ProcessLevyDeclarationsTimeoutCommand(Guid jobId)
        {
            JobId = jobId;
        }
    }
}