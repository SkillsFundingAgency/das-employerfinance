using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Web.RouteValues;
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
            {
                return;
            }

            // options: https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di

            var container = context.HttpContext.RequestServices.GetService<IContainer>();
            var employerUrls = container.GetInstance<IEmployerUrls>();

            var accountHashedId = (string)context.RouteData.Values[RouteValueKeys.AccountHashedId];
            //todo: temporary hack
            accountHashedId = "HASH";
            employerUrls.Initialize(accountHashedId);
            
            controller.ViewData["EmployerUrls"] = employerUrls;

            await next();
        }
    }
}