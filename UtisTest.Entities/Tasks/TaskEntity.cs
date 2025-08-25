using UtisTestTask.Entities.Enums;

namespace UtisTestTask.Entities.Tasks;

public class TaskEntity
{
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDateUtc { get; set; }
    public ScheduledTaskStatus Status { get; set; }
}