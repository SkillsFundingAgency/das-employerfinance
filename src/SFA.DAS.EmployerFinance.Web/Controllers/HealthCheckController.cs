using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerFinance.Application.Commands.RunHealthCheck;
using SFA.DAS.EmployerFinance.Application.Queries.GetHealthCheck;
using SFA.DAS.EmployerFinance.Web.RouteValues.HealthCheck;
using SFA.DAS.EmployerFinance.Web.ViewModels.HealthCheck;

namespace SFA.DAS.EmployerFinance.Web.Controllers
{
    [Route("healthcheck")]
    public class HealthCheckController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HealthCheckController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var query = new GetHealthCheckQuery();
            var response = await _mediator.Send(query);
            var model = _mapper.Map<HealthCheckViewModel>(response);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(HealthCheckRouteValues routeValues)
        {
            var command = new RunHealthCheckCommand();
            
            await _mediator.Send(command);

            return RedirectToAction("Index");
        }
    }
}