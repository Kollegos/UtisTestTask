using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using UtisTestTask.Consumer.Base;

namespace UtisTestTask.Consumer;

public class TaskOverdueConsumerService : IHostedService, IDisposable
{
    private readonly RabbitSettings _settings;
    private IConnection? _connection;
    private IChannel? _channel;

    public TaskOverdueConsumerService(RabbitSettings settings)
    {
        _settings = settings;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Delay to wait for all containers
        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
        var factory = new ConnectionFactory() { HostName = _settings.Host, Port = _settings.Port, UserName = _settings.UserName, Password = _settings.Password };
        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(queue: _settings.TaskOverdueQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null, cancellationToken: cancellationToken);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += OnMessageReceiveAsync;

        await _channel.BasicConsumeAsync(_settings.TaskOverdueQueue, autoAck: true, consumer: consumer, cancellationToken: cancellationToken);
    }

    private Task OnMessageReceiveAsync(object model, BasicDeliverEventArgs ea)
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message}");
        return Task.CompletedTask;
    }


    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel != null) await _channel.CloseAsync(cancellationToken: cancellationToken);
        if (_connection != null) await _connection.CloseAsync(cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}