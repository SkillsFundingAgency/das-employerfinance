using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.EmployerFinance.Web.Extensions;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public class DefaultStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasCookiePolicy()
                .AddDasMvc()
                .AddDasOidcAuthentication()
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