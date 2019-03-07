using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ProcessLevyDeclarations
{
    public class ProcessLevyDeclarationsCommand : IRequest
    {
        public DateTime PayrollPeriod { get; }

        public ProcessLevyDeclarationsCommand(DateTime payrollPeriod)
        {
            PayrollPeriod = payrollPeriod;
        }
    }
}