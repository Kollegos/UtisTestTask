using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UtisTestTask.Currency.Base;
using UtisTestTask.Currency.Services;

namespace UtisTestTask.Currency;

public static class CurrencyDependencyInjection
{
    public static IServiceCollection AddCurrency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ICurrencyService, CurrencyService>();
        services.AddMemoryCache(); 
        // Add Configuration
        var settingsSection = configuration.GetRequiredSection(CurrencyServiceOptions.ConfigurationSection);
        services.Configure<CurrencyServiceOptions>(settingsSection);
        var settings = settingsSection.Get<CurrencyServiceOptions>();
        if (string.IsNullOrEmpty(settings?.Url) || settings.CacheMinutes < 0) throw new ArgumentNullException(nameof(CurrencyServiceOptions), "Currency Service not configured in AppSetting. \"CurrencyServiceOptions\" section expected.");

        services.AddSingleton(settings);

        return services;
    }
    
}