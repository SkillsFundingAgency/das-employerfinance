using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}