using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstractions;
using Repositories.Implementations;
using Services.Abstractions;
using Services.Implementations;
using WebApi.Extensions;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;
        // Add services to the container.
        services.AddDbContext<TaskPointsDbContext>(
            opt => opt.UseNpgsql(
                configuration.GetConnectionString(nameof(TaskPointsDbContext)),
                options => options.MigrationsAssembly(typeof(TaskPointsDbContext).Assembly)));

        services.AddScoped<ITaskPointsService, TaskPointsService>();
        services.AddScoped<IReadTaskPointsRepository, TaskPointsRepository>();
        services.AddScoped<IWriteTaskPointsRepository, TaskPointsRepository>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.MigrateDatabase<TaskPointsDbContext>();

        app.Run();
    }
}