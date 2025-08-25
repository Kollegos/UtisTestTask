using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace UtisTestTask.Core.Base.Validators;

internal static class ValidatorExtensions
{
    /// <summary>
    /// Check if primary key exists in DbSet
    /// </summary>
    /// <typeparam name="TContext">DbContext type</typeparam>
    /// <typeparam name="TProperty">Property type</typeparam>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <param name="ruleBuilder">Validation rule builder</param>
    /// <param name="dbSet">Db set</param>
    /// <returns></returns>
    internal static IRuleBuilderOptions<TContext, TProperty> ExistsInTable<TContext, TProperty, TEntity>(
        this IRuleBuilder<TContext, TProperty> ruleBuilder,
        DbSet<TEntity> dbSet
    )
        where TEntity : class
        => ruleBuilder.SetValidator(new ExistsInTableByIdValidator<TContext, TProperty, TEntity>(dbSet));
}