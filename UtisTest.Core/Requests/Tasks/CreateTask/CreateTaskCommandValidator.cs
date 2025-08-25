using FluentValidation;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.CreateTask;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommandData>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(_ => _.Title).Length(1, DatabaseSettings.MaxTitleLength);
        RuleFor(_ => _.Description).Length(0, DatabaseSettings.MaxDescriptionLength);
        RuleFor(_ => _.DueDateUtc).GreaterThan(DateTime.UtcNow);
    }
}