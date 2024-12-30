using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework;

public class TaskPointsDbContext(DbContextOptions<TaskPointsDbContext> options) : DbContext(options)
{
    public DbSet<TaskPoint> TaskPoints { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}