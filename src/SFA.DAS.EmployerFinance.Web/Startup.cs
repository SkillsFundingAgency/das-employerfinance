using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerFinance.HealthChecks;
using SFA.DAS.EmployerFinance.Startup;
using SFA.DAS.EmployerFinance.Web.DependencyResolution;
using SFA.DAS.EmployerFinance.Web.Filters;
using SFA.DAS.EmployerFinance.Web.HealthChecks;
using SFA.DAS.UnitOfWork.Mvc;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web
{
    public class Startup
    {
        private IContainer _container;
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(o =>
            {
                o.CheckConsentNeeded = c => true;
                o.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddMvc(o =>
                {
                    o.Filters.Add(new UrlsViewBagFilter());
                })
                .AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks()
                .AddSqlServer(_configuration["ConnectionStrings:DefaultConnection"])
                .AddCheck<ApiHealthCheck>(
                    "Employer Finance Api",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] {"ready"})
                .AddCheck<NServiceBusHealthCheck>(
                    "Employer Finance NServiceBus",
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new[] {"ready"});

            _container = IoC.Initialize(services);
            
            var startup = _container.GetInstance<IRunAtStartup>();
            var serviceProvider = _container.GetInstance<IServiceProvider>();
            
            startup.StartAsync().GetAwaiter().GetResult();
            
            return serviceProvider;
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
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseUnitOfWork();
            app.UseMvc();
            
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = (check) => check.Tags.Contains("ready"),
                ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
            });
        }
    }
}