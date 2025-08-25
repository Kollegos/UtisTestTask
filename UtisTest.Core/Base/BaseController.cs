using Microsoft.AspNetCore.Mvc;
using UtisTestTask.Core.Base.Requests;

namespace UtisTestTask.Core.Base;

public abstract class BaseController : ControllerBase
{
    protected IRequestHandler RequestHandler;
    protected BaseController(IRequestHandler requestHandler) => this.RequestHandler = requestHandler;
}