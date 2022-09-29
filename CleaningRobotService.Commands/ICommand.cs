using System.Windows.Input;

namespace CleaningRobotService.Commands
{
    public interface ICommandHandler<T>{
        void Execute(T command);
    }

}