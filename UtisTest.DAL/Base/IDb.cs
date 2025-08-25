namespace UtisTestTask.DAL.Base;

public interface IDb
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}