using EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Repositories.Abstractions;
using Repositories.Implementations;
using Services.Implementations.Handlers.QueryHandlers;
using Services.Implementations.Mapping;
using WebApi.Extensions;
using WebApi.Mapping;

namespace WebApi;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;
        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString(nameof(TaskPointsDbContext));
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"The connection string {nameof(TaskPointsDbContext)} cannot be null or empty.");
        // Add services to the container.
        services.AddDbContext<TaskPointsDbContext>(
            opt => opt.UseNpgsql(
                configuration.GetConnectionString(nameof(TaskPointsDbContext)),
                options => options.MigrationsAssembly(typeof(TaskPointsDbContext).Assembly)));

        services.AddScoped<IReadTaskPointsRepository, TaskPointsRepository>();
        services.AddScoped<IWriteTaskPointsRepository, TaskPointsRepository>();

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<TaskPointsApplicationProfile>();
            cfg.AddProfile<TaskPointsPresentationProfile>();
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllTaskPointsWithFilterQueryHandler).Assembly));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Task Points API",
                Version = "v1",
                Description = "API to manage task points in a task management system."
            });

            var xmlFile = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
            if (File.Exists(xmlFile))
            {
                options.IncludeXmlComments(xmlFile);
            }
        }); var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Points API v1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        await app.MigrateDatabase<TaskPointsDbContext>();

        await app.RunAsync();
    }
}