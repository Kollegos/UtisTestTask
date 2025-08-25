using JetBrains.Annotations;
using UtisTestTask.Entities.Enums;

namespace UtisTestTask.Core.Requests.Tasks.GetTasks;

[PublicAPI]
public class GetTasksQueryData
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public ScheduledTaskStatus? Status { get; set; }
}