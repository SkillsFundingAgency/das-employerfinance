using System;
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
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.EmployerFinance.Web.Authentication;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.EmployerFinance.Web.Filters;
using SFA.DAS.UnitOfWork.Mvc;
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

        private IContainer _container;
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(o =>
                {
                    o.CheckConsentNeeded = c => true;
                    o.MinimumSameSitePolicy = SameSiteMode.None;
                })
            // ConfigureContainer() hasn't been called yet, so we have to get the Oidc config from IConfiguration, rather than serviceProvider
            .AddAndConfigureAuthentication(Configuration.GetEmployerFinanceSection<OidcConfiguration>("Oidc"))
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

            _container = IoC.Initialize(services);

            var startup = _container.GetInstance<IRunAtStartup>();
            startup.StartAsync().GetAwaiter().GetResult();

            return _container.GetInstance<IServiceProvider>();
        }

        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime, IHostingEnvironment env)
        {
            applicationLifetime.ApplicationStopping.Register(() =>
            {
                using (_container)
                {
                    _container.GetInstance<IRunAtStartup>().StopAsync().GetAwaiter().GetResult();
                }
            });
            
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
    }
}