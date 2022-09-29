using CleaningRobotService.Commands.Repository;
using CleaningRobotService.Model;

namespace CleaningRobotService.Commands
{
    public class UniqueCoordinatesVisitedCommand 
    {
        public (int x, int y) StartCoordinates { get; set; }
        public (Direction direction, int steps)[] MoveCommands { get; set; } = Array.Empty<(Direction direction, int steps)>();
    }
    public class UniqueCoordinatesVisitedCommandHandler : ICommandHandler<UniqueCoordinatesVisitedCommand, int>
    {
        private readonly IExecutionsRepository executionsRepository;

        public UniqueCoordinatesVisitedCommandHandler(IExecutionsRepository executionsRepository)
        {
            this.executionsRepository = executionsRepository;
        }

        public async Task<int> RunAsync(UniqueCoordinatesVisitedCommand command)
        {
            var result = UniqueCoordinatesVisitedHelper.RunCalculations(command);

            if (result.IsCompleted)
            {
                return await executionsRepository.AddExecutionAsync(new DataModel.Execution
                {
                    Commands = command.MoveCommands.Length,
                    Result = Calculations.GetTotalUniqueCoordinates(),
                    TimeStamp = DateTime.UtcNow,
                    Duration = 2394
                });
            }

            return -1;
        }
    }


}
