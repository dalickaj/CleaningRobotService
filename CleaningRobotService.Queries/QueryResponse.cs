namespace CleaningRobotService.Commands
{
    public class QueryResponse<Out>
    {
        public Out Data { get; set; }
        public string Error { get; set; }
    }


}
