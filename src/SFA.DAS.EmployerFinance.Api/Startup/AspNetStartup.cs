using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Api.DependencyResolution;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Api.Startup
{
    public class AspNetStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDasMvc();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDasCultureInfo()
                .UseDasHsts()
                .UseHttpsRedirection()
                .UseMvc();
        }
    }
}