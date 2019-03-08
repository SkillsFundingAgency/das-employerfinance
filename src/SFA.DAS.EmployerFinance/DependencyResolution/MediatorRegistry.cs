using MediatR;
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
                s.AssembliesAndExecutablesFromApplicationBaseDirectory();
                s.ConnectImplementationsToTypesClosing(typeof(IPipelineBehavior<,>));
                s.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
            });
        }
    }
}