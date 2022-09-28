using System.Windows.Input;

namespace CleaningRobotService.Commands
{
    public interface ICommand<T>{
        void Execute(T command);
    }
}