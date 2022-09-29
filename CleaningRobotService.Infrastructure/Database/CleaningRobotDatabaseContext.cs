using CleaningRobotService.DataModel;
using Microsoft.EntityFrameworkCore;

namespace CleaningRobotService.Infrastructure.Database
{
    public class CleaningRobotDatabaseContext : DbContext
    {
        public CleaningRobotDatabaseContext(DbContextOptions<CleaningRobotDatabaseContext> options)
        : base(options) { }
        public DbSet<Execution> Executions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql("Host=localhost;Database=postgres;Username=postgres;Password=admin");
    }
}
