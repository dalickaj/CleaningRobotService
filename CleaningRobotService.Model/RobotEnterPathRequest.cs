using CleaningRobotService.Model;

namespace CleaningRobotService.ApiModel
{
    public class RobotEnterPathRequest
    {
        public Coordinates Coordinates { get; set; } = new Coordinates();
        public RobotCommand[] Commands { get; set; } = Array.Empty<RobotCommand>();
    }
}
