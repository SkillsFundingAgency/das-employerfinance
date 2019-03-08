using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Api.DependencyResolution;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Startup;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Startup
{
    public class AspNetStartup
    {
        private readonly EmployerFinanceConfiguration _employerFinanceConfiguration;

        public AspNetStartup(IConfiguration configuration)
        {
            _employerFinanceConfiguration = configuration.GetEmployerFinanceSection<EmployerFinanceConfiguration>();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasHealthChecks(_employerFinanceConfiguration.DatabaseConnectionString)
                .AddDasMvc();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDasCultureInfo()
                .UseDasHsts()
                .UseHttpsRedirection()
                .UseDasHealthChecks()
                .UseMvc();
        }
    }
}