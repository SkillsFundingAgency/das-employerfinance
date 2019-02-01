using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Configuration;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Filters
{
    public class GoogleAnalyticsViewBagFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.Controller is Controller controller))
            {
                return;
            }
            
            var container = context.HttpContext.RequestServices.GetService<IContainer>();
            var configuration = container.GetInstance<GoogleAnalyticsConfiguration>();
            controller.ViewBag.GoogleAnalyticsConfiguration = configuration;
        }
    }
}