using JetBrains.Annotations;

namespace UtisTestTask.Core.Requests.Tasks.DeleteTask;

[PublicAPI]
public class DeleteTaskCommandData
{
    public Guid TaskId { get; set; }
}