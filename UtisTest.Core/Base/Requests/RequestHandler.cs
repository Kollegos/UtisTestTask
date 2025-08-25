using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UtisTestTask.Core.Base.Requests;

public class RequestHandler : IRequestHandler
{
    /// <summary>
    /// Validate data and handle request
    /// </summary>
    /// <typeparam name="TData">Request data type</typeparam>
    /// <typeparam name="TResult">Request result type</typeparam>
    /// <param name="request">Request handler</param>
    /// <param name="data">Request data</param>
    /// <returns></returns>
    public async Task<ActionResult<TResult>> Handle<TData, TResult>(IRequest<TData, TResult> request, TData data)
    {
        var result = request.Validate(data);
        if (result.IsValid)
        {
            return await request.Handle(data);
        }

        var errors = new ModelStateDictionary();
        foreach (var error in result.Errors)
        {
            errors.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return new BadRequestObjectResult(errors);
    }
}