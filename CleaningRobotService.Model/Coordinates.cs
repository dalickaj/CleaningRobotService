using CleaningRobotService.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.Model
{
    public class Coordinates
    {
        [Range(Constant.MIN_X, Constant.MAX_X)]
        public int X { get; set; } = 0;

        [Range(Constant.MIN_Y, Constant.MAX_Y)]
        public int Y { get; set; } = 0;
    }
}