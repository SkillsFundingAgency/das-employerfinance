using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
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
        //todo: how to do it this way and still have access to config in startup?
//        public static IContainer Initialize(IEnumerable<ServiceDescriptor> descriptors, IConfiguration configuration)
//        {
//            return new Container(c =>
//            {
//                c.AddRegistry(new ConfigurationRegistry(configuration));
//                c.AddRegistry<DataRegistry>();
//                c.AddRegistry<EmployerFinanceApiClientRegistry>();
//                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
//                c.AddRegistry<LoggingRegistry>();
//                c.AddRegistry<MapperRegistry>();
//                c.AddRegistry<MediatorRegistry>();
//                c.AddRegistry<NServiceBusClientUnitOfWorkRegistry>();
//                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
//                c.AddRegistry<StartupRegistry>();
//                c.AddRegistry<DefaultRegistry>();
//                c.Populate(descriptors);
//            });
//        }

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