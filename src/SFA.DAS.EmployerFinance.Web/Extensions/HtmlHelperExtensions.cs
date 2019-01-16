using Microsoft.AspNetCore.Mvc.Rendering;
using SFA.DAS.EmployerFinance.Web.Urls;

namespace SFA.DAS.EmployerFinance.Web.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IEmployerUrls EmployerUrls(this IHtmlHelper htmlHelper)
        {
            return (IEmployerUrls)htmlHelper.ViewData["EmployerUrls"];
        }
    }
}