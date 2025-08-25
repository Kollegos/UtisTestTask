namespace UtisTestTask.Currency.Services;

public interface ICurrencyService
{
    Task<string> GetCurrency();
}