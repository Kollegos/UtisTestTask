using JetBrains.Annotations;
using UtisTestTask.Entities.Enums;

namespace UtisTestTask.Core.Requests.Tasks.UpdateTask;

[PublicAPI]
public class UpdateTaskCommandData
{
    public Guid TaskId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public ScheduledTaskStatus Status { get; set; }
}