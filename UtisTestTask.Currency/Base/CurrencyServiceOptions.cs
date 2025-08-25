namespace UtisTestTask.Currency.Base;

public class CurrencyServiceOptions
{
    public const string ConfigurationSection = "CurrencyServiceOptions";
    public string Url { get; set; } = "";
    public int CacheMinutes { get; set; }
}