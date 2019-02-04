using System.Threading.Tasks;
using MediatR;
using NServiceBus;
using SFA.DAS.EmployerFinance.Application.Commands.ReceiveEmployerFinanceHealthCheckEvent;
using SFA.DAS.EmployerFinance.Messages.Events;

namespace SFA.DAS.EmployerFinance.MessageHandlers.EventHandlers.EmployerFinance
{
    public class HealthCheckEventHandler : IHandleMessages<HealthCheckEvent>
    {
        private readonly IMediator _mediator;

        public HealthCheckEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Handle(HealthCheckEvent message, IMessageHandlerContext context)
        {
            return _mediator.Send(new ReceiveEmployerFinanceHealthCheckEventCommand(message.Id));
        }
    }
}