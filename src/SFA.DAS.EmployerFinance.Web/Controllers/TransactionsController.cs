using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles;
using SFA.DAS.Authorization.Mvc;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [DasAuthorize(EmployerUserRole.Any)]
    [Route("accounts/{accountHashedId}/transactions")]
    public class TransactionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}