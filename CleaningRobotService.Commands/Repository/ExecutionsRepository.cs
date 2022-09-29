using CleaningRobotService.DataModel;
using CleaningRobotService.Infrastructure.Database;

namespace CleaningRobotService.Commands.Repository
{
    public interface IExecutionsRepository
    {
        public Task<int> AddExecutionAsync(Execution entry);
    }
    public class ExecutionsRepository : IExecutionsRepository
    {
        private readonly CleaningRobotDatabaseContext db;
        public ExecutionsRepository(CleaningRobotDatabaseContext db)
        {
            this.db = db;
        }

        public async Task<int> AddExecutionAsync(Execution entry)
        {
            db.Add(entry);
            await db.SaveChangesAsync();

            return entry.Id;
        }
    }
}
