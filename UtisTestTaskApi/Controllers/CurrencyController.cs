using Microsoft.AspNetCore.Mvc;
using UtisTestTask.Core.Base;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.Core.Requests.Currency;

namespace UtisTestTask.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CurrencyController : BaseController
    {

        public CurrencyController(IRequestHandler requestHandler) : base(requestHandler)
        {
        }


        [HttpGet]
        public Task<ActionResult<string>> GetCurrency([FromQuery] GetCurrencyQueryData data, [FromServices] GetCurrencyQuery command)
            => RequestHandler.Handle(command, data);
    }
}
