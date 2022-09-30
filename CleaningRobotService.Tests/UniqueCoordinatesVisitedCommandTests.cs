using CleaningRobotService.Commands;
using CleaningRobotService.Commands.Repository;
using Moq;

namespace CleaningRobotService.Tests
{
    public class UniqueCoordinatesVisitedCommandTests
    {
        private readonly Mock<IExecutionsRepository> repositoryMock;
        private readonly UniqueCoordinatesVisitedCommandHandler sut;
        public UniqueCoordinatesVisitedCommandTests()
        {
            repositoryMock = new Mock<IExecutionsRepository>();
            repositoryMock.Setup(x => x.AddExecutionAsync(It.IsAny<DataModel.Execution>()))
                .ReturnsAsync(1);

            sut = new UniqueCoordinatesVisitedCommandHandler(repositoryMock.Object);
        }

        [Theory]
        [InlineData(0, 0, Model.Direction.North, 1, 2)]
        [InlineData(0, 0, Model.Direction.North, 2, 3)]
        [InlineData(-100000, -10000, Model.Direction.East, 2, 3)]
        [InlineData(100000, 100000, Model.Direction.South, 100, 101)]
        public async Task RunAsync_OneMoveComandAsync(int x, int y, Model.Direction direction, int steps, int uniqueCoordinatesCount)
        {
            await sut.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = (x, y),
                MoveCommands = new (Model.Direction direction, int steps)[]
                 {
                     (direction, steps)
                 }
            });

            repositoryMock.Verify(x => x.AddExecutionAsync(It.Is<DataModel.Execution>(r => r.Result == uniqueCoordinatesCount)), Times.Once);
        }

        [Fact]
        public async Task RunAsync_MoreMoveCommands()
        {
            await sut.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = (0, 0),
                MoveCommands = new (Model.Direction direction, int steps)[]
                 {
                     (Model.Direction.North, 100),
                     (Model.Direction.East, 100),
                     (Model.Direction.South, 100),
                     (Model.Direction.West, 100),
                     (Model.Direction.North, 100),
                 }
            });

            repositoryMock.Verify(x => x.AddExecutionAsync(It.Is<DataModel.Execution>(r => r.Result == 400)), Times.Once);
        }

        [Theory]
        [InlineData(1500)]
        public async Task RunAsync_ManyMoveCommands(int commandCount)
        {
            var northMove = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.North, 1)).ToList();
            var southMove = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.South, 1)).ToList();
            northMove.AddRange(southMove);

            await sut.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = (0, 0),
                MoveCommands = northMove.ToArray()
            });

            repositoryMock.Verify(x => x.AddExecutionAsync(It.Is<DataModel.Execution>(r => r.Result == 1501)), Times.Once);
        }

        [Theory]
        [InlineData(2000)]
        public async Task RunAsync_ManyMoveCommandsInThreeDirections(int commandCount)
        {
            var move = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.North, 1)).ToList();
            var southMove = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.South, 1)).ToList();
            var eastMove = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.East, 1)).ToList();
            var westMove = Enumerable.Range(0, commandCount).Select(command => (Model.Direction.West, 1)).ToList();

            move.AddRange(southMove);
            move.AddRange(eastMove);
            move.AddRange(westMove);

            await sut.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = (6, 7),
                MoveCommands = move.ToArray()
            });

            repositoryMock.Verify(x => x.AddExecutionAsync(It.Is<DataModel.Execution>(r => r.Result == 4001)), Times.Once);
        }

        [Theory]
        [InlineData(200, 50, 3, 7, 261)]
        public async Task RunAsync_BigStepMoves(int northSteps, int eastSteps, int southSteps, int westSteps, int result)
        {

            await sut.RunAsync(new UniqueCoordinatesVisitedCommand
            {
                StartCoordinates = (6, 7),
                MoveCommands = new (Model.Direction direction, int steps)[]
                {
                    (Model.Direction.North, northSteps),
                     (Model.Direction.East, eastSteps),
                     (Model.Direction.South, southSteps),
                     (Model.Direction.West, westSteps)
                }
            });

            repositoryMock.Verify(x => x.AddExecutionAsync(It.Is<DataModel.Execution>(r => r.Result == result)), Times.Once);
        }
    }
}