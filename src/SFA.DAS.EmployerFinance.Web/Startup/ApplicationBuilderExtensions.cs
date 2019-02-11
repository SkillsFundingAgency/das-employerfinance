using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using SFA.DAS.UnitOfWork.Mvc;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDasCultureInfo(this IApplicationBuilder app)
        {
            var cultureInfo = new CultureInfo("en-GB");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            
            return app;
        }

        public static IApplicationBuilder UseDasErrorPages(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
            
            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            return app;
        }

        public static IApplicationBuilder UseDasHsts(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
            
            if (!hostingEnvironment.IsDevelopment())
            {
                app.UseHsts();
            }

            return app;
        }

        public static IApplicationBuilder UseDasUnitOfWork(this IApplicationBuilder app)
        {
            return app.UseUnitOfWork();
        }
    }
}