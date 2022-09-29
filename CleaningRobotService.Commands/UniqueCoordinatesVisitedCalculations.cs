using CleaningRobotService.Model;

namespace CleaningRobotService.Commands
{
    internal class UniqueCoordinatesVisitedCalculations
    {
        private static readonly int chunkSize = 1000;

        private readonly Calculations calcuations;
        public UniqueCoordinatesVisitedCalculations()
        {
            calcuations = new Calculations();
        }
        public IEnumerable<Task> RunCalculations(UniqueCoordinatesVisitedCommand command)
        {
            var totalCommands = command.MoveCommands.Count();
            int totalChunks = totalCommands / chunkSize + 1;
            var commandSegments = SplitCommandIntoSegments(command, totalCommands, totalChunks);
            var chunkCoordinates = SetStartCoordinatesForEachSegment(command, totalChunks, commandSegments);

            var tasks = Enumerable.Range(0, totalChunks).Select(chunkNumber =>
               Task.Run(() => calcuations.SetUniqueCoordinates(chunkCoordinates[chunkNumber], commandSegments[chunkNumber])));

            return tasks;
        }

        public int GetTotalUniqueCoordinates()
        {
            return calcuations.GetTotalUniqueCoordinates();
        }

        private static (int x, int y)[] SetStartCoordinatesForEachSegment(UniqueCoordinatesVisitedCommand command, int totalChunks, ArraySegment<(Direction direction, int steps)>[] commandSegments)
        {
            var chunkCoordinates = new (int x, int y)[totalChunks];
            chunkCoordinates[0] = command.StartCoordinates;

            for (int chunkNumber = 1; chunkNumber < totalChunks; chunkNumber++)
            {
                chunkCoordinates[chunkNumber] = Calculations.MultipleStepsMove(chunkCoordinates[chunkNumber - 1], commandSegments[chunkNumber]);
            }

            return chunkCoordinates;
        }

        private static ArraySegment<(Direction direction, int steps)>[] SplitCommandIntoSegments(UniqueCoordinatesVisitedCommand command, int totalCommands, int totalChunks)
        {
            return Enumerable.Range(0, totalChunks).Select(chunkNumber =>
            {
                int fromIndex = chunkNumber * chunkSize;
                int count = totalCommands < chunkSize ? totalCommands : chunkSize;
                return new ArraySegment<(Direction direction, int steps)>(command.MoveCommands, fromIndex, count);
            }).ToArray();
        }
    }
}