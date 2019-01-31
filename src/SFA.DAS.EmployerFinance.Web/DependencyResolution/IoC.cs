using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.AutoConfiguration.DependencyResolution;
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
        public static IContainer Initialize(IEnumerable<ServiceDescriptor> descriptors)
        {
            return new Container(c =>
            {
                c.AddRegistry<AutoConfigurationRegistry>();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EmployerFinanceApiClientRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
                c.AddRegistry<LoggingRegistry>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusClientUnitOfWorkRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
                c.Populate(descriptors);
            });
        }
    }
}