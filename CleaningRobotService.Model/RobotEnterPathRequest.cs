using CleaningRobotService.Model;
using System.ComponentModel.DataAnnotations;

namespace CleaningRobotService.ApiModel
{
    public class RobotEnterPathRequest
    {
        public Coordinates Coordinates { get; set; } = new Coordinates();

        [MaxLength(10000,ErrorMessage ="Max number of commands exceeded!")]
        public RobotCommand[] Commands { get; set; } = Array.Empty<RobotCommand>();
    }
}
