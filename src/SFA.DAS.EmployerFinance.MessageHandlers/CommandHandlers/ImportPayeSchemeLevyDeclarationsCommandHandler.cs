using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ImportPayeSchemeLevyDeclarations;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ImportPayeSchemeLevyDeclarationsCommandHandler : IHandleMessages<ImportPayeSchemeLevyDeclarationsCommand>
    {
        private readonly IMediator _mediator;

        public ImportPayeSchemeLevyDeclarationsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(ImportPayeSchemeLevyDeclarationsCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}