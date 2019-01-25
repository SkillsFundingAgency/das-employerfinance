using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.Extensions;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.EmployerFinance.Web.Filters;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            })
            // ConfigureContainer() hasn't been called yet, so we have to get the Oidc config from IConfiguration, rather than serviceProvider
            .AddAuthenticationService(serviceProvider.GetService<IHostingEnvironment>(), Configuration.GetEmployerFinanceSection<OidcConfiguration>("Oidc"))
            .AddMvc(options =>
            {
                options.Filters.Add(new UrlsViewBagFilter());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
        
        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry, Configuration);
        }
    }
}