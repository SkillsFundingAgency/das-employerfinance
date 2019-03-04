using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SFA.DAS.EmployerFinance.Web.Extensions
{
    public static class ControllerExtensions
    {
        public static bool ViewExists(this Controller controller, ICompositeViewEngine viewEngine, string name)
        {
            var result = viewEngine.FindView(controller.ControllerContext, name, false);
            return (result.View != null);
        }
    }
}