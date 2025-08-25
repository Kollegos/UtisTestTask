using JetBrains.Annotations;

namespace UtisTestTask.Entities.Enums;

[PublicAPI]
public enum ScheduledTaskStatus
{
    Undefined = 0,
    New = 1,
    InProgress = 2,
    Completed = 3,
    Overdue = 4,
}