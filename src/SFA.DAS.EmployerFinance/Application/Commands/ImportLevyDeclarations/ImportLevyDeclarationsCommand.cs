using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations
{
    public class ImportLevyDeclarationsCommand : IRequest
    {
        public Guid JobId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }

        public ImportLevyDeclarationsCommand(Guid jobId, DateTime payrollPeriod, long accountPayeSchemeId)
        {
            JobId = jobId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
        }
    }
}