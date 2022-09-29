using System.Windows.Input;

namespace CleaningRobotService.Commands
{
    public interface IQueryHandler<In,Out>{
        Task<QueryResponse<Out>> GetAsync(In query);
    }

}