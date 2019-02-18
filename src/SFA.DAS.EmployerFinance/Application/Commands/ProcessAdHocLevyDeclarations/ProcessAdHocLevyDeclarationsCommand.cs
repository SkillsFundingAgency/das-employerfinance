using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessAdHocLevyDeclarations
{
    public class ProcessAdHocLevyDeclarationsCommand : IRequest
    {
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }

        public ProcessAdHocLevyDeclarationsCommand(DateTime payrollPeriod, long accountPayeSchemeId)
        {
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
        }
    }
}