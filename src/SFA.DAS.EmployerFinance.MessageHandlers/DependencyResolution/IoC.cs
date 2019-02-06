using Microsoft.Extensions.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;
using SFA.DAS.EmployerFinance.Data;
using SFA.DAS.UnitOfWork.EntityFrameworkCore;
using SFA.DAS.UnitOfWork.NServiceBus;

namespace SFA.DAS.EmployerFinance.MessageHandlers.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(string environmentName, IConfiguration configuration)
        {
            return new Container(c =>
            {
                c.For<IConfiguration>().Use(configuration).Singleton();
                c.AddRegistry<ConfigurationRegistry>();
                c.AddRegistry(new DasNonMvcHostingEnvironmentRegistry(environmentName));
                c.AddRegistry<DataRegistry>();
                c.AddRegistry<EntityFrameworkCoreUnitOfWorkRegistry<EmployerFinanceDbContext>>();
                c.AddRegistry<MapperRegistry>();
                c.AddRegistry<MediatorRegistry>();
                c.AddRegistry<NServiceBusUnitOfWorkRegistry>();
                c.AddRegistry<StartupRegistry>();
                c.AddRegistry<DefaultRegistry>();
            });
        }
    }
}
