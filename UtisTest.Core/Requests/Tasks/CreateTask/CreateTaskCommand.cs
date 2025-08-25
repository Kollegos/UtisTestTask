using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.DAL.Base;
using UtisTestTask.Entities.Enums;
using UtisTestTask.Entities.Tasks;

namespace UtisTestTask.Core.Requests.Tasks.CreateTask;

[PublicAPI]
public class CreateTaskCommand : BaseRequest<CreateTaskCommandData, Guid>
{
    private readonly ITasksDb _db;

    public CreateTaskCommand(ILogger<CreateTaskCommand> logger, ITasksDb db, CreateTaskCommandValidator validator) : base(logger, validator)
    {
        _db = db;
    }

    public override async Task<Guid> Handle(CreateTaskCommandData data, CancellationToken ct = default)
    {
        Log.LogTrace("Create task begin");
        var entry = await _db.Tasks.AddAsync(new TaskEntity
        {
            Title = data.Title,
            Description = data.Description,
            DueDateUtc = data.DueDateUtc,
            Status = ScheduledTaskStatus.New
        }, ct);

        await _db.SaveChangesAsync(ct);

        Log.LogTrace("Create task end");
        return entry.Entity.TaskId;
    }
}