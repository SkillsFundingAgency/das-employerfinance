using System;
using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations
{
    public class ImportLevyDeclarationsCommand : IRequest
    {
        public int SagaId { get; }
        public DateTime PayrollPeriod { get; }
        public long AccountPayeSchemeHighWaterMarkId { get; }

        public ImportLevyDeclarationsCommand(int sagaId, in DateTime payrollPeriod, long accountPayeSchemeHighWaterMarkId)
        {
            SagaId = sagaId;
            PayrollPeriod = payrollPeriod;
            AccountPayeSchemeHighWaterMarkId = accountPayeSchemeHighWaterMarkId;
        }
    }
}