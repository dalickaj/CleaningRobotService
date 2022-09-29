
using CleaningRobotService.DataModel;
using CleaningRobotService.Queries.Repository;

namespace CleaningRobotService.Commands
{
    public class GetUniqueCoordinatesExecutionsQuery
    {
        public int Id { get; set; }
    }

    public class UniqueCoordinatesExecutionsQueryHandler : IQueryHandler<GetUniqueCoordinatesExecutionsQuery, Execution>
    {
        private readonly IExecutionsRepository executionsRepository;
        public UniqueCoordinatesExecutionsQueryHandler(IExecutionsRepository executionsRepository)
        {
            this.executionsRepository = executionsRepository;
        }

        public async Task<QueryResponse<Execution>> GetAsync(GetUniqueCoordinatesExecutionsQuery query)
        {
            var result = await executionsRepository.GetExecutionAsync(query.Id);

            return new QueryResponse<Execution>
            {
                Data = result
            };
        }
    }


}
