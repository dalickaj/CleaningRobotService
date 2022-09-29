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
    }
}