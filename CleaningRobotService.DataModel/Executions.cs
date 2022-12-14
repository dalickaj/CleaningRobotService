namespace CleaningRobotService.DataModel
{
    public class Execution
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Commands { get; set; }
        public int Result { get; set; }
        public double Duration { get; set; }
    }
}