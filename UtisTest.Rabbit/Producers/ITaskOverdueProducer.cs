namespace UtisTestTask.Rabbit.Producers;


public interface ITaskOverdueProducer
{
    /// <summary>
    /// Send task overdue messages to rabbit
    /// </summary>
    /// <param name="messages">Messages</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task SendMessages(IEnumerable<string> messages, CancellationToken ct = default);
}