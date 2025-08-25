using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using UtisTestTask.Core.Base.Requests;
using UtisTestTask.Currency.Services;

namespace UtisTestTask.Core.Requests.Currency;

[PublicAPI]
public class GetCurrencyQuery : BaseRequest<GetCurrencyQueryData, string>
{
    private readonly ICurrencyService _currencyService;

    public GetCurrencyQuery(ILogger<GetCurrencyQuery> logger, ICurrencyService currencyService) : base(logger, null)
    {
        _currencyService = currencyService;
    }

    public override async Task<string> Handle(GetCurrencyQueryData data, CancellationToken ct = default)
    {
        return await _currencyService.GetCurrency();
    }
}