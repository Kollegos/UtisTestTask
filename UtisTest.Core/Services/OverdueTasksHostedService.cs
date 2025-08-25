using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UtisTestTask.DAL.Base;
using UtisTestTask.Entities.Enums;
using UtisTestTask.Rabbit.Messages;
using UtisTestTask.Rabbit.Producers;

namespace UtisTestTask.Core.Services;

public class OverdueTasksHostedService : IHostedService, IDisposable
{
    private const int ServicePeriodMinute = 1;
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    private Timer? _timer;

    public OverdueTasksHostedService(ILogger<OverdueTasksHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("OverdueTasksHostedService is starting.");

        try
        {
            _timer = new Timer(UpdateOverdueTasksStatus, null, TimeSpan.Zero, TimeSpan.FromMinutes(ServicePeriodMinute));

            _logger.LogInformation("OverdueTasksHostedService is started.");
        }
        catch (Exception e)
        {
            _logger.LogError(default, e, "OverdueTasksHostedService was not started.");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("OverdueTasksHostedService is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async void UpdateOverdueTasksStatus(object? state)
    {
        try
        {
            _logger.LogTrace("UpdateOverdueTasksStatus executing.");
            using var scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<ITasksDb>();

            var messages = new List<TaskOverdueMessage>();
            var tasks = db.Tasks.Where(_ => _.DueDateUtc < DateTime.UtcNow && _.Status != ScheduledTaskStatus.Overdue).ToList();
            foreach (var taskEntity in tasks)
            {
                taskEntity.Status = ScheduledTaskStatus.Overdue;
                messages.Add(new TaskOverdueMessage{TaskId = taskEntity.TaskId});
            }

            var updated = await db.SaveChangesAsync();

            await SendTaskOverdueMessages(messages, scope);

            _logger.LogTrace($"UpdateOverdueTasksStatus complete. {updated} rows updated.");
        }
        catch (Exception e)
        {
            _logger.LogError(default, e, "UpdateOverdueTasksStatus failed.");
        }
    }

    private async Task SendTaskOverdueMessages(IReadOnlyCollection<TaskOverdueMessage> messages, IServiceScope scope)
    {
        try
        {
            var messageProducer = scope.ServiceProvider.GetRequiredService<ITaskOverdueProducer>();
            if (messages.Any()) await messageProducer.SendMessages(messages.Select(JsonConvert.SerializeObject));
        }
        catch (Exception e)
        {
            _logger.LogError(default, e, "SendTaskOverdueMessages failed.");
        }
    }
}