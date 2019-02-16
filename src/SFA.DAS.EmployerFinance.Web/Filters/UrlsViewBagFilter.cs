using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using SFA.DAS.EmployerFinance.Web.RouteValues;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.Filters
{
    public class UrlsViewBagFilter : IAsyncActionFilter
    {
        private readonly IEmployerUrls _employerUrls;

        public UrlsViewBagFilter(IEmployerUrls employerUrls)
        {
            _employerUrls = employerUrls;
        }
        
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var accountHashedId = (string)context.HttpContext.GetRouteValue(RouteValueKeys.AccountHashedId);
            var controller = (Controller)context.Controller;
            
            _employerUrls.Initialize(accountHashedId);

            controller.ViewData["EmployerUrls"] = _employerUrls;

            return next();
        }
    }
}