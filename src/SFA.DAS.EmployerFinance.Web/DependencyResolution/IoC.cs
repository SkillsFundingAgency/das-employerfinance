using SFA.DAS.EmployerFinance.Api.Client.DependencyResolution;
using SFA.DAS.EmployerFinance.Data;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.NServiceBus;
using SFA.DAS.UnitOfWork.NServiceBus.ClientOutbox;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<EmployerFinanceApiClientRegistry>();
            registry.IncludeRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
            registry.IncludeRegistry<LoggerRegistry>();
            registry.IncludeRegistry<MapperRegistry>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<NServiceBusClientUnitOfWorkRegistry>();
            registry.IncludeRegistry<NServiceBusUnitOfWorkRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}