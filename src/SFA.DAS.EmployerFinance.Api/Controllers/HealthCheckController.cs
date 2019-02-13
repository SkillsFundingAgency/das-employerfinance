using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [Route("healthcheck")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}