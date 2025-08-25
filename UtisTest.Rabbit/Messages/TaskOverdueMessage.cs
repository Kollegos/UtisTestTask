namespace UtisTestTask.Rabbit.Messages;

public class TaskOverdueMessage
{
    public string MessageCode => "TaskOverdue";
    public Guid TaskId { get; set; }
}