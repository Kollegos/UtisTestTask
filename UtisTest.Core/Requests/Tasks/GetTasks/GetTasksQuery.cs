using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.GetTasks;

[PublicAPI]
public class GetTasksQuery : BaseRequest<GetTasksQueryData, GetTasksQueryResult>
{
    private readonly ITasksDb _db;

    public GetTasksQuery(ILogger<GetTasksQuery> logger, ITasksDb db, GetTasksQueryValidator validator) : base(logger, validator)
    {
        _db = db;
    }

    public override async Task<GetTasksQueryResult> Handle(GetTasksQueryData data, CancellationToken ct = default)
    {
        Log.LogTrace($"Get tasks begin. Page {data.Page}. Page Size {data.PageSize}. Status {data.Status}.");
        var totalElements = await _db.Tasks.AsNoTracking().Where(_ => _.Status == data.Status || data.Status == null).CountAsync(ct);
        var elements = await _db.Tasks.AsNoTracking().Where(_ => _.Status == data.Status || data.Status == null).Skip(data.Page * data.PageSize).Take(data.PageSize).ToListAsync(ct);

        Log.LogTrace($"Get tasks end. Return {elements.Count} elements.");
        return new GetTasksQueryResult
        {
            TotalElements = totalElements,
            Tasks = elements.Select(_ => new GetTasksQueryResultTask
            {
                TaskId = _.TaskId,
                Title = _.Title,
                Description = _.Description,
                Status = _.Status,
                DueDateUtc = _.DueDateUtc
            }).ToList()
        };
    }
}