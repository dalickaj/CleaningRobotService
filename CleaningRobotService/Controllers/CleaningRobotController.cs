using CleaningRobotService.ApiModel;
using CleaningRobotService.Commands;
using CleaningRobotService.DataModel;
using Microsoft.AspNetCore.Mvc;

namespace CleaningRobotService.Controllers
{
    [ApiController]
    [Route("tibber-developer-test")]
    public class CleaningRobotController : ControllerBase
    {
        private readonly ILogger<CleaningRobotController> logger;
        private readonly ICommandHandler<UniqueCoordinatesVisitedCommand, int> command;
        private readonly IQueryHandler<GetUniqueCoordinatesExecutionsQuery, Execution> query;
        public CleaningRobotController(ILogger<CleaningRobotController> logger, 
            ICommandHandler<UniqueCoordinatesVisitedCommand, int> command,
            IQueryHandler<GetUniqueCoordinatesExecutionsQuery, Execution> query)
        {
            this.logger = logger;
            this.command = command;
            this.query = query;
        }

        [HttpPost("enter-path")]
        public async Task<ExecutionsResponse> SaveUniqueVisitedCoordinates(RobotEnterPathRequest enterPathRequest)
        {
           int executionId = await command.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = new(enterPathRequest.Coordinates.X, enterPathRequest.Coordinates.Y),
                MoveCommands = enterPathRequest.Commands.Select(x => (x.Direction, x.Steps)).ToArray()
            });

            var result = await query.GetAsync(new GetUniqueCoordinatesExecutionsQuery { Id = executionId });

            if (!string.IsNullOrEmpty(result.Error)) return new ExecutionsResponse();

            return new ExecutionsResponse
            {
                Id = executionId,
                Commands = result.Data.Commands,
                Result = result.Data.Result,
                Duration = result.Data.Duration,
                TimeStamp = result.Data.TimeStamp
            };
        }
    }
}