using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using UtisTestTask.Currency.Base;

namespace UtisTestTask.Currency.Services;

internal class CurrencyService : ICurrencyService
{
    private const string CacheKey = "CurencyList";
    private readonly IMemoryCache _cache;
    private readonly CurrencyServiceOptions _options;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly ILogger _logger;

    public CurrencyService(IMemoryCache cache, CurrencyServiceOptions options, ILogger<CurrencyService> logger)
    {
        _cache = cache;
        _options = options;
        _logger = logger;
        
        _retryPolicy = Policy<HttpResponseMessage>
            .HandleResult(response => !response.IsSuccessStatusCode) // Handle not success
            .Or<HttpRequestException>() // Handle exceptions
            .WaitAndRetryAsync(
                retryCount: 3, // Retries
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // Exponential pause between requests
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    _logger.LogError($"Retry #{retryAttempt} failed.  Wait for {timespan}.  Reason: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                });
    }

    public async Task<string> GetCurrency()
    {
        try
        {
            _logger.LogTrace("Start getting currency");
            // Try to get from cache
            if (_cache.TryGetValue(CacheKey, out string? json))
            {
                return json ?? "";
            }


            _logger.LogTrace($"Currency cache is empty. Trying to get from external resource: {_options.Url}.");
            using var client = new HttpClient();
            var response = await _retryPolicy.ExecuteAsync(async () => await client.GetAsync(_options.Url));
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();
            ;

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_options.CacheMinutes));

            // Set the value in cache
            _cache.Set(CacheKey, json, cacheEntryOptions);

            return json;

        }
        catch (Exception e)
        {
            throw new WebException($"Can't get currencies from remote API {_options.Url}", e);
        }
        finally
        {

            _logger.LogTrace("End getting currency");
        }
    }
}