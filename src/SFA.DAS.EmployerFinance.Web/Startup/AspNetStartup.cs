using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.UnitOfWork.Mvc;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public class AspNetStartup
    {
        private readonly OidcConfiguration _oidcConfiguration;

        public AspNetStartup(IConfiguration configuration)
        {
            _oidcConfiguration = configuration.GetEmployerFinanceSection<OidcConfiguration>("Oidc");
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasCookiePolicy()
                .AddDasMvc()
                .AddDasNServiceBus()
                .AddDasOidcAuthentication(_oidcConfiguration);
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
                .UseCookiePolicy()
                .UseAuthentication()
                .UseUnitOfWork()
                .UseMvc();
        }
    }
}