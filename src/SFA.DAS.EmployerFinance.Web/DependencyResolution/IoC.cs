using Microsoft.Extensions.Configuration;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;
using SFA.DAS.EmployerFinance.DependencyResolution;

namespace SFA.DAS.EmployerFinance.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            //            var employerFinanceConfig = new EmployerFinanceConfiguration();
//            Configuration.GetSection("SFA.DAS.EmployerFinanceV2").Bind(employerFinanceConfig);

//todo: let consumers work using IConfiguration, or insert into container?
//            var x = configuration.GetSection("SFA.DAS.EmployerFinanceV2").Get<EmployerFinanceConfiguration>();
//
//            registry.For<>()
            
            registry.IncludeRegistry<ConfigurationRegistry>();
            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}