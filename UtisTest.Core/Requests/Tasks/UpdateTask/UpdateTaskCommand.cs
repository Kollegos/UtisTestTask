using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.UpdateTask;

[PublicAPI]
public class UpdateTaskCommand : BaseRequest<UpdateTaskCommandData, Guid>
{
    private readonly ITasksDb _db;

    public UpdateTaskCommand(ILogger<UpdateTaskCommand> logger, ITasksDb db, UpdateTaskCommandValidator validator) : base(logger, validator)
    {
        _db = db;
    }

    public override async Task<Guid> Handle(UpdateTaskCommandData data, CancellationToken ct = default)
    {
        Log.LogTrace($"Update task begin. Id: {data.TaskId}");
        var task = await _db.Tasks.FirstOrDefaultAsync(_ => _.TaskId == data.TaskId, ct);
        if (task == null) throw new ValidationException("Task not found.");

        task.Title = data.Title;
        task.Description = data.Description;
        task.Status = data.Status;

        await _db.SaveChangesAsync(ct);

        Log.LogTrace("Update task end");
        return task.TaskId;
    }
}