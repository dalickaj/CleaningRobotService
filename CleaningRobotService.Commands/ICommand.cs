using System.Windows.Input;

namespace CleaningRobotService.Commands
{
    public interface ICommandHandler<In,Out>{
        Task<Out> RunAsync(In command);
    }

}