using CleaningRobotService.ApiModel;
using CleaningRobotService.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Controllers
{
    [ApiController]
    [Route("tibber-developer-test")]
    public class CleaningRobotController : ControllerBase
    {
        private readonly ILogger<CleaningRobotController> logger;
        private readonly ICommandHandler<UniqueCoordinatesVisitedCommand> command;
        public CleaningRobotController(ILogger<CleaningRobotController> logger, 
            ICommandHandler<UniqueCoordinatesVisitedCommand> command)
        {
            this.logger = logger;
            this.command = command;
        }

        [HttpPost("enter-path")]
        public ExecutionsResponse SaveUniqueVisitedCoordinates(RobotEnterPathRequest enterPathRequest)
        {
            command.Execute(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = new(enterPathRequest.Coordinates.X, enterPathRequest.Coordinates.Y),
                MoveCommands = enterPathRequest.Commands.Select(x => (x.Direction, x.Steps)).ToArray()
            });

            return new ExecutionsResponse();
        }
    }
}