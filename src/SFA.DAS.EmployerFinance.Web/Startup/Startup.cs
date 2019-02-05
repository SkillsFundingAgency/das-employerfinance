using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using SFA.DAS.EmployerFinance.Configuration.Extensions;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.EmployerFinance.Web.Filters;
using SFA.DAS.UnitOfWork.Mvc;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    //todo: integrate config changes from https://github.com/SkillsFundingAgency/das-employerfinance/blob/master/src/SFA.DAS.EmployerFinance.Web/Startup.cs
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
            
            services.Configure<CookiePolicyOptions>(o =>
            {
                o.CheckConsentNeeded = c => true;
                o.MinimumSameSitePolicy = SameSiteMode.None;
            })
            // ConfigureContainer() hasn't been called yet, so we have to get the Oidc config from IConfiguration, rather than serviceProvider
            .AddAndConfigureAuthentication(serviceProvider.GetService<IHostingEnvironment>(), Configuration.GetEmployerFinanceSection<OidcConfiguration>("Oidc"))
            .AddMvc(o =>
            {
                //todo: inject directly into view rather than using filter?
                o.Filters.Add(new UrlsViewBagFilter());
                
                // default to all pages/actions requiring authentication and allow opt-out with [AllowAnonymous], rather than opting in with [Authorize]
                //todo: put in helper?
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            })
            .AddControllersAsServices()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //todo: how to marry up this with needing config in this method (for setting up auth)
//            var startup = _container.GetInstance<IRunAtStartup>();
//            var serviceProvider = _container.GetInstance<IServiceProvider>();
//            
//            startup.StartAsync().GetAwaiter().GetResult();
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
            app.UseUnitOfWork();
            app.UseMvc();
        }
        
        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry, Configuration);
        }
    }
}