using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.EmployerFinance.Web.Startup
{
    public static class ErrorPagesStartup
    {
        public static IApplicationBuilder UseDasErrorPages(this IApplicationBuilder app)
        {
            var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
            
            if (hostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error.html");
            }

            return app.UseStatusCodePagesWithReExecute("/{0}.html")
                .UseUnauthorizedAccessExceptionHandler();
        }
    }
}