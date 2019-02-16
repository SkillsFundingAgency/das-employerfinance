using MediatR;

namespace SFA.DAS.EmployerFinance.Application.Commands.ReceiveEmployerFinanceHealthCheckEvent
{
    public class ReceiveEmployerFinanceHealthCheckEventCommand : IRequest
    {
        public int Id { get; }

        public ReceiveEmployerFinanceHealthCheckEventCommand(int id)
        {
            Id = id;
        }
    }
}