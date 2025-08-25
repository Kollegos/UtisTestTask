using System.Text;
using RabbitMQ.Client;
using UtisTestTask.Rabbit.Base;

namespace UtisTestTask.Rabbit.Producers;

internal class TaskOverdueProducer : ITaskOverdueProducer
{
    private readonly RabbitSettings _settings;
    public TaskOverdueProducer(RabbitSettings settings)
    {
        _settings = settings;
    }

    /// <summary>
    /// Send task overdue messages to rabbit
    /// </summary>
    /// <param name="messages">Messages</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public async Task SendMessages(IEnumerable<string> messages, CancellationToken ct = default)
    {
        var factory = new ConnectionFactory() { HostName = _settings.Host, Port = _settings.Port, UserName = _settings.UserName, Password = _settings.Password };

        var connection = await factory.CreateConnectionAsync(ct);
        await using (connection)
        {
            var channel = await connection.CreateChannelAsync(null, ct);
            await using (channel)
            {
                await channel.QueueDeclareAsync(queue: _settings.TaskOverdueQueue,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    cancellationToken: ct);

                foreach (var message in messages)
                {
                    var body = Encoding.UTF8.GetBytes(message);

                    await channel.BasicPublishAsync(exchange: "",
                        routingKey: _settings.TaskOverdueQueue,
                        body: body,
                        cancellationToken: ct);
                }
            }
        }
    }
}