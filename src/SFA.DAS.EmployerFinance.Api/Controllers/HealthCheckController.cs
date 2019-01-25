using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.EmployerFinance.Api.Controllers
{
    [Route("api/[controller]")]
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