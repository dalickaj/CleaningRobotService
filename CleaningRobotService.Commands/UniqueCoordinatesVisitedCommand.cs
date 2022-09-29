using CleaningRobotService.Model;

namespace CleaningRobotService.Commands
{
    public class UniqueCoordinatesVisitedCommand 
    {
        public (int x, int y) StartCoordinates { get; set; }
        public (Direction direction, int steps)[] MoveCommands { get; set; } = Array.Empty<(Direction direction, int steps)>();
    }
    public class UniqueCoordinatesVisitedCommandHandler : ICommandHandler<UniqueCoordinatesVisitedCommand>
    {
        private static readonly int chunkSize = 1000;
        public void Execute(UniqueCoordinatesVisitedCommand command)
        {
            var totalCommands = command.MoveCommands.Count();
            int totalChunks = totalCommands / chunkSize + 1;

            var commandSegments = Enumerable.Range(0, totalChunks).Select(chunkNumber => {
                int fromIndex = chunkNumber * chunkSize;
                int count = totalCommands < chunkSize ? totalCommands : chunkSize;
                return new ArraySegment<(Direction direction, int steps)>(command.MoveCommands, fromIndex, count);
            }).ToArray();

            var chunkCoordinates = new (int x, int y)[totalChunks];
            chunkCoordinates[0] = command.StartCoordinates;

            for(int chunkNumber = 1; chunkNumber < totalChunks; chunkNumber++)
            {
                chunkCoordinates[chunkNumber] = Calculations.MultipleStepsMove(chunkCoordinates[chunkNumber - 1], commandSegments[chunkNumber]);
            }

            var result = Parallel.For(0, totalChunks, (chunkNumber) =>
            {
                Calculations.SetUniqueCoordinates(chunkCoordinates[chunkNumber], commandSegments[chunkNumber]);
            });

            if (result.IsCompleted)
            {
                SaveCountResult(Calculations.GetTotalUniqueCoordinates(), command.MoveCommands.Length);
            }
        }

        public void SaveCountResult(int count, int numberOfSteps)
        {
            throw new NotImplementedException();
        }


    }


}
