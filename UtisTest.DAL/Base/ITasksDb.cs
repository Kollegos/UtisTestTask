using Microsoft.EntityFrameworkCore;
using UtisTestTask.Entities.Tasks;

namespace UtisTestTask.DAL.Base;

public interface ITasksDb : IDb
{
    DbSet<TaskEntity> Tasks { get; set; }
}