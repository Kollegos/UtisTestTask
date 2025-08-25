using FluentValidation.Results;

namespace UtisTestTask.Core.Base.Requests;

public interface IRequest<in TData, TResult>
{
    /// <summary>
    /// Validate request data
    /// </summary>
    /// <param name="data">Request data</param>
    /// <returns>Validation result</returns>
    ValidationResult Validate(TData data);

    /// <summary>
    /// Handle request
    /// </summary>
    /// <param name="data">Request data</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<TResult> Handle(TData data, CancellationToken ct = default);
}