using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations
{
    public class ImportLevyDeclarationsCommand : IRequest
    {
        public int SagaId { get; }

        public ImportLevyDeclarationsCommand(int sagaId)
        {
            SagaId = sagaId;
        }
    }
}