using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
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
                .AddDasMvc()
                .AddDasOidcAuthentication(_employerFinanceConfiguration.Oidc)
                .AddDasNServiceBus();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication()
                .UseDasCultureInfo()
                .UseHttpsRedirection()
                .UseDasHsts()
                .UseDasErrorPages()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseDasUnitOfWork()
                .UseMvc();
        }
    }
}