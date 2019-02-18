using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ImportLevyDeclarations;

namespace SFA.DAS.EmployerFinance.MessageHandlers.CommandHandlers
{
    public class ImportLevyDeclarationsCommandHandler : IHandleMessages<ImportLevyDeclarationsCommand>
    {
        private readonly IMediator _mediator;

        public ImportLevyDeclarationsCommandHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        public Task Handle(ImportLevyDeclarationsCommand message, IMessageHandlerContext context)
        {
            return _mediator.Send(message);
        }
    }
}