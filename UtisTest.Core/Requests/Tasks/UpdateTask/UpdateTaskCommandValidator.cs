using FluentValidation;
using UtisTestTask.Core.Base.Validators;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.UpdateTask;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommandData>
{
    public UpdateTaskCommandValidator(ITasksDb db)
    {
        RuleFor(_ => _.TaskId).ExistsInTable(db.Tasks);
        RuleFor(_ => _.Title).Length(1, DatabaseSettings.MaxTitleLength);
        RuleFor(_ => _.Description).Length(0, DatabaseSettings.MaxDescriptionLength);
    }
}