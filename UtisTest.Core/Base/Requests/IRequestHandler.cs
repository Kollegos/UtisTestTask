using Microsoft.AspNetCore.Mvc;

namespace UtisTestTask.Core.Base.Requests;

public interface IRequestHandler
{
    /// <summary>
    /// Validate data and handle request
    /// </summary>
    /// <typeparam name="TData">Request data type</typeparam>
    /// <typeparam name="TResult">Request result type</typeparam>
    /// <param name="request">Request handler</param>
    /// <param name="data">Request data</param>
    /// <returns></returns>
    Task<ActionResult<TResult>> Handle<TData, TResult>(IRequest<TData, TResult> request, TData data);
}