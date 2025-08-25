using JetBrains.Annotations;

namespace UtisTestTask.Core.Requests.Tasks.CreateTask;

[PublicAPI]
public class CreateTaskCommandData
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime DueDateUtc { get; set; }
}