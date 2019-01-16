using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.EmployerFinance.Web.Urls;
using StructureMap;

namespace SFA.DAS.EmployerFinance.Web.Filters
{
    public class UrlsViewBagFilter : IAsyncActionFilter, IAsyncPageFilter
    {
//        private readonly Func<IEmployerUrls> _employerUrls;
//        private readonly ILogger _logger;

//        public UrlsViewBagFilter(IServiceProvider serviceProvider, Func<IEmployerUrls> employerUrls/*, ILogger logger*/)
//        {
//            _employerUrls = employerUrls;
//
//            _employerUrls = (Func<IEmployerUrls>)serviceProvider.GetRequiredService<IEmployerUrls>();
////            _logger = logger;
//        }

        public UrlsViewBagFilter(IContainer container/*, ILogger logger*/)
        {
            //_employerUrls = container.GetInstance<Func<IEmployerUrls>>();
//            _logger = logger;
        }

        public async Task OnActionExecutionAsync
            (ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var container = context.HttpContext.RequestServices.GetService<IContainer>();
            var x_employerUrls = container.GetInstance<Func<IEmployerUrls>>();
            
            var controller = context.Controller as Controller;
            if (controller == null)
                return;

            var employerUrls = x_employerUrls();
            //var employerUrls = _employerUrls();
            controller.ViewData["EmployerUrls"] = employerUrls;

            var resultContext = await next();
        }
        
        public async Task OnPageHandlerSelectionAsync(
            PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }

        public async Task OnPageHandlerExecutionAsync(
            PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {
            //var employerUrls = _employerUrls();
//            var accountHashedId = (string)context.RouteData.Values[RouteValueKeys.AccountHashedId];
            
//            employerUrls.Initialize(accountHashedId);
            
//            context.HttpContext.Controller.ViewBag.EmployerUrls = employerUrls;

            var page = context.HandlerInstance as PageModel;
            if (page == null)
                return;
            
            //page.ViewData["EmployerUrls"] = employerUrls;
            
            await next.Invoke();
        }
    }
}