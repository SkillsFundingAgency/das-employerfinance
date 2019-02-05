using Microsoft.Extensions.Configuration;
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
        public static void Initialize(Registry registry, IConfiguration configuration)
        {
            registry.IncludeRegistry(new ConfigurationRegistry(configuration));
            registry.IncludeRegistry<DataRegistry>();
            registry.IncludeRegistry<EmployerFinanceApiClientRegistry>();
            registry.IncludeRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
            registry.IncludeRegistry<LoggingRegistry>();
            registry.IncludeRegistry<MapperRegistry>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<NServiceBusClientUnitOfWorkRegistry>();
            registry.IncludeRegistry<NServiceBusUnitOfWorkRegistry>();
            registry.IncludeRegistry<StartupRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}