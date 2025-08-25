using FluentValidation;
using UtisTestTask.Core.Base.Validators;
using UtisTestTask.DAL.Base;

namespace UtisTestTask.Core.Requests.Tasks.DeleteTask;

public class DeleteTaskCommandValidator : AbstractValidator<DeleteTaskCommandData>
{
    public DeleteTaskCommandValidator(ITasksDb db)
    {
        RuleFor(_ => _.TaskId).ExistsInTable(db.Tasks);
    }
}