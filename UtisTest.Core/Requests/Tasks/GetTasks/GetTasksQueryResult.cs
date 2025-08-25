using JetBrains.Annotations;
using UtisTestTask.Entities.Enums;

namespace UtisTestTask.Core.Requests.Tasks.GetTasks;

[PublicAPI]
public class GetTasksQueryResult
{
    public int TotalElements { get; set; }
    public List<GetTasksQueryResultTask> Tasks { get; set; } = new();
}

[PublicAPI]
public class GetTasksQueryResultTask
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DueDateUtc { get; set; }
    public ScheduledTaskStatus Status { get; set; }
}
