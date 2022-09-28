using CleaningRobotService.ApiModel;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Controllers
{
    [ApiController]
    [Route("tibber-developer-test")]
    public class CleaningRobotController : ControllerBase
    {
        private readonly ILogger<CleaningRobotController> _logger;

        public CleaningRobotController(ILogger<CleaningRobotController> logger)
        {
            _logger = logger;
        }

        [HttpPost("enter-path")]
        public ExecutionsResponse SaveUniqueVisitedCoordinates(RobotEnterPathRequest enterPathRequest)
        {
            return new ExecutionsResponse();
        }
    }
}