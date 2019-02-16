using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                app.UseExceptionHandler("/Home/Error");
            }

            return app;
        }
    }
}