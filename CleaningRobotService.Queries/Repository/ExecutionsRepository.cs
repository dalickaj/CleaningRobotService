using CleaningRobotService.DataModel;
using CleaningRobotService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.Queries.Repository
{
    public interface IExecutionsRepository
    {
        public Task<Execution> GetExecutionAsync(int id);
    }
    public class ExecutionsRepository : IExecutionsRepository
    {
        private readonly CleaningRobotDatabaseContext db;
        public ExecutionsRepository(CleaningRobotDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<Execution> GetExecutionAsync(int id)
        {
            return await db.Executions.FirstAsync(x => x.Id == id);
        }
    }
}
