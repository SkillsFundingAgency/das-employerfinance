using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.NServiceBus;

namespace SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
            registry.IncludeRegistry<MapperRegistry>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<NServiceBusUnitOfWorkRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}
