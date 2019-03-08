using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations
{
    public class ImportPayeSchemeLevyDeclarationsCommand : IRequest
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeId { get; }

        public ImportPayeSchemeLevyDeclarationsCommand(int sagaId, DateTime payrollPeriod, long accountPayeSchemeId)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeId = accountPayeSchemeId;
        }
    }
}