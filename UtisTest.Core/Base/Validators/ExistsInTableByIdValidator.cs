using FluentValidation;
using FluentValidation.Validators;
using Microsoft.EntityFrameworkCore;

namespace UtisTestTask.Core.Base.Validators;

public class ExistsInTableByIdValidator<TContext, TProperty, TEntity>: PropertyValidator<TContext, TProperty>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet;
    public ExistsInTableByIdValidator(DbSet<TEntity> dbSet) => _dbSet = dbSet;

    public override string Name => nameof(ExistsInTableByIdValidator<TContext, TProperty, TEntity>);

    public override bool IsValid(ValidationContext<TContext> context, TProperty property)
    {
        context.MessageFormatter.AppendArgument("ObjectType", typeof(TEntity).Name);
        context.MessageFormatter.AppendArgument("Value", property);

        if (property == null)
            return false;

        var entity = _dbSet.Find(property);
        return entity != null;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) => "'{ObjectType}' with '{PropertyName}' = '{Value}' was not found.";
}