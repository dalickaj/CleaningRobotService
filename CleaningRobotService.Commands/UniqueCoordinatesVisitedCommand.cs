using CleaningRobotService.Commands.Repository;
using CleaningRobotService.Model;
using System.Diagnostics;

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
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var calculations = new UniqueCoordinatesVisitedCalculations();
            var calculationTasks = calculations.RunCalculations(command);
            await Task.WhenAll(calculationTasks);

            int result = calculations.GetTotalUniqueCoordinates();

            stopWatch.Stop();

            return await executionsRepository.AddExecutionAsync(new DataModel.Execution
            {
                Commands = command.MoveCommands.Length,
                Result = result,
                TimeStamp = DateTime.UtcNow,
                Duration = stopWatch.Elapsed.TotalMilliseconds
            });
        }
    }


}
