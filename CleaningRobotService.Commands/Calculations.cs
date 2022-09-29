﻿using CleaningRobotService.Infrastructure;
using CleaningRobotService.Model;

namespace CleaningRobotService.Commands
{
    public static class Calculations
    {

        private static readonly int xMaxLength = (Constant.MAX_X - Constant.MIN_X) / Constant.SQUARE_SIZE + 1;
        private static readonly int yMaxLength = (Constant.MAX_Y - Constant.MIN_Y) / Constant.SQUARE_SIZE + 1;
        private static readonly HashSet<(int, int)>[,] alreadyVisited = new HashSet<(int, int)>[xMaxLength, yMaxLength]; 
        
        public static int GetTotalUniqueCoordinates()
        {
            int total = 0;
            for(int i = 0; i < xMaxLength; i++)
            {
                for(int j = 0; j < yMaxLength; j++)
                {
                    if (alreadyVisited[i, j] == null) continue;
                    total += alreadyVisited[i, j].Count;
                }
            }

            return total;
        }

        public static void SetUniqueCoordinates((int x, int y) startCoordinates, ArraySegment<(Direction direction, int steps)> moveCommands)
        {
            var currentCoordinates = startCoordinates;
            SetAlreadyVisited(alreadyVisited, currentCoordinates);

            for (int i = 0; i < moveCommands.Count; i++)
            {
                currentCoordinates = Move(currentCoordinates, moveCommands[i].direction, moveCommands[i].steps);
                SetAlreadyVisited(alreadyVisited, currentCoordinates);
            };
        }

        public static (int x, int y) MultipleStepsMove((int x, int y) startCoordinates, ArraySegment<(Direction direction, int steps)> stepsSegment)
        {
            var northSteps = stepsSegment.Where(x => x.direction == Direction.North).Sum(x => x.steps);
            var southSteps = stepsSegment.Where(x => x.direction == Direction.South).Sum(x => x.steps);
            var eastSteps = stepsSegment.Where(x => x.direction == Direction.East).Sum(x => x.steps);
            var westSteps = stepsSegment.Where(x => x.direction == Direction.West).Sum(x => x.steps);

            return Calculations.Shift(startCoordinates, xShift: eastSteps - westSteps, yShift: northSteps - southSteps);
        }

        public static (int x, int y) Move((int x, int y) coordinates, Direction direction, int steps)
        {
            switch (direction)
            {
                case Direction.North: { coordinates.y += steps; break; }
                case Direction.East: { coordinates.x += steps; break; }
                case Direction.South: { coordinates.y -= steps; break; }
                case Direction.West: { coordinates.x -= steps; break; }
                default: throw new ArgumentOutOfRangeException();
            }

            if (!IsValidCoordinate(coordinates)) 
                throw new ArgumentOutOfRangeException($"Coordinates are out of range: ({coordinates.x},{coordinates.y})");

            return coordinates;
        }

        public static (int x, int y) Shift((int x, int y) coordinates, int xShift, int yShift)
        {
            return (coordinates.x + xShift, coordinates.y + yShift);
        }

        private static void SetAlreadyVisited(HashSet<(int, int)>[,] alreadyVisited, (int x, int y) coordinates)
        {
            int xSquareIndex = (coordinates.x - Constant.MIN_X) / Constant.SQUARE_SIZE;
            int ySquareIndex = (coordinates.y - Constant.MIN_Y) / Constant.SQUARE_SIZE;

            if (alreadyVisited[xSquareIndex, ySquareIndex] == null) 
                alreadyVisited[xSquareIndex, ySquareIndex] = new HashSet<(int, int)>();

            if (alreadyVisited[xSquareIndex, ySquareIndex].TryGetValue(coordinates, out _)) return;

            alreadyVisited[xSquareIndex, ySquareIndex].Add(coordinates);
        }

        private static bool IsValidCoordinate((int x, int y) coordinates)
        {
            return coordinates.x <= Constant.MAX_X
                && coordinates.y <= Constant.MAX_Y
                && coordinates.x >= Constant.MIN_X
                && coordinates.y >= Constant.MIN_Y;
        }

    }


}
