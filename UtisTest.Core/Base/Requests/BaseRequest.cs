using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace UtisTestTask.Core.Base.Requests;

public abstract class BaseRequest<TData, TResult> : IRequest<TData, TResult>
{
    protected readonly ILogger Log;
    protected readonly IValidator<TData>? Validator;

    protected BaseRequest(ILogger logger, IValidator<TData>? validator)
    {
        Log = logger;
        Validator = validator;
    }

    /// <summary>
    /// Validate request data
    /// </summary>
    /// <param name="data">Request data</param>
    /// <returns>Validation result</returns>
    public virtual ValidationResult Validate(TData data) => Validator?.Validate(data) ?? new ValidationResult();

    /// <summary>
    /// Handle request
    /// </summary>
    /// <param name="data">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public abstract Task<TResult> Handle(TData data, CancellationToken ct = default);
}