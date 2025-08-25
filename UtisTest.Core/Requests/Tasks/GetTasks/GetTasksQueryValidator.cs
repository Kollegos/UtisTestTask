using FluentValidation;

namespace UtisTestTask.Core.Requests.Tasks.GetTasks;

public class GetTasksQueryValidator : AbstractValidator<GetTasksQueryData>
{
    public GetTasksQueryValidator()
    {
        RuleFor(_ => _.Page).GreaterThanOrEqualTo(0);
        RuleFor(_ => _.PageSize).GreaterThanOrEqualTo(1);
    }
}