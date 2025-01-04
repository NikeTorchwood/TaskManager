using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;
public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabase<T>(this IHost host)
        where T : DbContext
    {
        var scope = host.Services.CreateScope();
        var appContext = scope.ServiceProvider.GetService<T>();
        await appContext?.Database.MigrateAsync();
        return host;
    }
}