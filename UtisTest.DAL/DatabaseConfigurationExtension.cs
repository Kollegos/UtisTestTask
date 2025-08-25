using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.DAL;

public static class DatabaseConfigurationExtension
{
    /// <summary>
    /// Configure database
    /// </summary>
    /// <param name="service">Services</param>
    /// <param name="configuration">Configuration data</param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDatabase(this IServiceCollection service,
        IConfiguration configuration)
    {
        service.AddDbContext<UtisTestContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(typeof(DatabaseConfigurationExtension).Assembly));
        });

        service.AddScoped<ITasksDb, UtisTestContext>();

        return service;
    }

    public static void MigrateDatabase(this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<UtisTestContext>();
        context?.Database.Migrate();
    }
}