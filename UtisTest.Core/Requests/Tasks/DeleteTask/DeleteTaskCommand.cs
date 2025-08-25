using FluentValidation;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.DeleteTask;

[PublicAPI]
public class DeleteTaskCommand : BaseRequest<DeleteTaskCommandData, EmptyResult>
{
    private readonly ITasksDb _db;

    public DeleteTaskCommand(ILogger<DeleteTaskCommand> logger, ITasksDb db, DeleteTaskCommandValidator validator) : base(logger, validator)
    {
        _db = db;
    }

    public override async Task<EmptyResult> Handle(DeleteTaskCommandData data, CancellationToken ct = default)
    {
        Log.LogTrace($"Delete task begin. Id: {data.TaskId}");
        var task = await _db.Tasks.FirstOrDefaultAsync(_ => _.TaskId == data.TaskId, ct);
        if (task == null) throw new ValidationException("Task not found.");
        _db.Tasks.Remove(task);

        await _db.SaveChangesAsync(ct);

        Log.LogTrace("Delete task end");
        return new EmptyResult();
    }
}