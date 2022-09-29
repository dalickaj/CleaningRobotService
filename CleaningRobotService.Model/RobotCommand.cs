using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.Model
{
    public class RobotCommand
    {
        public Direction Direction { get; set; }

        [Range(1, 100000, ErrorMessage = "Steps can be between 1 and 100000!")]
        public int Steps { get; set; }
    }
}