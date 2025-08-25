using Microsoft.Extensions.Hosting;
using UtisTestTask.DAL;

namespace UtisTestTask.Core;

public static class CoreDatabaseExtension
{
    public static void MigrateApplicationDatabase(this IHost app)
    {
        app.MigrateDatabase();
    }
}