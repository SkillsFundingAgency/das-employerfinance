using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarationsAdHoc
{
    public class ProcessLevyDeclarationsAdHocCommand : IRequest
    {
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }

        public ProcessLevyDeclarationsAdHocCommand(DateTime payrollPeriod, long accountPayeSchemeId)
        {
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
        }
    }
}