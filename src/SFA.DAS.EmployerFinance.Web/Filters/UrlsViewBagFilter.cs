using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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

            var container = context.HttpContext.RequestServices.GetService<IContainer>();
            controller.ViewData["EmployerUrls"] = container.GetInstance<IEmployerUrls>();

            await next();
        }
    }
}