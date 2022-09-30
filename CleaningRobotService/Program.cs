using CleaningRobotService.Commands;
using CleaningRobotService.DataModel;
using CleaningRobotService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
RegisterServices(builder);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<CleaningRobotDatabaseContext>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("default"));
    });


    builder.Services.AddScoped<ICommandHandler<UniqueCoordinatesVisitedCommand, int>, UniqueCoordinatesVisitedCommandHandler>();
    builder.Services.AddScoped<IQueryHandler<GetUniqueCoordinatesExecutionsQuery, Execution>, UniqueCoordinatesExecutionsQueryHandler>();
    
    builder.Services.AddScoped<CleaningRobotService.Commands.Repository.IExecutionsRepository, CleaningRobotService.Commands.Repository.ExecutionsRepository>();
    builder.Services.AddScoped<CleaningRobotService.Queries.Repository.IExecutionsRepository, CleaningRobotService.Queries.Repository.ExecutionsRepository>();
}