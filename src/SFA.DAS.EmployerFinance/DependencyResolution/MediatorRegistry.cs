using MediatR;
using SFA.DAS.EmployerFinance.Application.Commands.RunHealthCheck;
using StructureMap;

namespace SFA.DAS.EmployerFinance.DependencyResolution
{
    public class MediatorRegistry : Registry
    {
        public MediatorRegistry()
        {
            For<IMediator>().Use<Mediator>();
            For<ServiceFactory>().Use<ServiceFactory>(c => c.GetInstance);
            
            Scan(s =>
            {
                s.AssemblyContainingType<RunHealthCheckCommandHandler>();
                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });
        }
    }
}