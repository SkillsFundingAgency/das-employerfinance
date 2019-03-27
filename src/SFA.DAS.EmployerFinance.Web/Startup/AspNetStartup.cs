using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.UnitOfWork.Mvc;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
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
            services.AddDasCookiePolicy()
                .AddDasHealthChecks(_employerFinanceConfiguration.DatabaseConnectionString)
                .AddDasMvc()
                .AddDasNServiceBus()
                .AddDasOidcAuthentication(_employerFinanceConfiguration.Oidc)
                .AddHttpsRedirection(o => o.HttpsPort = 5001);
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseDasCultureInfo()
                .UseDasErrorPages()
                .UseHttpsRedirection()
                .UseDasHsts()
                .UseStaticFiles()
                .UseDasHealthChecks()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseUnitOfWork()
                .UseMvc();
        }
    }
}