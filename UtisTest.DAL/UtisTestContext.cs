using System.Reflection;
using Microsoft.EntityFrameworkCore;
using UtisTestTask.DAL.Base;
using UtisTestTask.Entities.Tasks;

namespace UtisTestTask.DAL;

public class UtisTestContext : DbContext, ITasksDb
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor for tests
    /// </summary>
    /// <param name="connectionString"></param>
    public UtisTestContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public UtisTestContext(DbContextOptions<UtisTestContext> options)
        : base(options)
    {
    }

    // Tasks
    public virtual DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var dbAssembly = Assembly.GetAssembly(typeof(UtisTestContext));
        if (dbAssembly != null) modelBuilder.ApplyConfigurationsFromAssembly(dbAssembly);
    }

    /// <summary>
    /// For tests
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!string.IsNullOrEmpty(_connectionString))
            optionsBuilder.UseNpgsql(_connectionString);
    }
}