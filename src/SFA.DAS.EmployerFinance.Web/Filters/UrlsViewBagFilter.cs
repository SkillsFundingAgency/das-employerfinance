using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Web.Urls;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Filters
{
    public class UrlsViewBagFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller == null)
                return;

            // options: https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di

            //todo: no need for func if we get it each time, rather than using ctor injection
            var container = context.HttpContext.RequestServices.GetService<IContainer>();
            var getEmployerUrls = container.GetInstance<Func<IEmployerUrls>>();

            controller.ViewData["EmployerUrls"] = getEmployerUrls();

            await next();
        }
    }
}