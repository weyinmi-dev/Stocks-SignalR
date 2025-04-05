using System.Globalization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Stocks_SignalR.Stocks;

public class StocksClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<StocksClient> _logger;

    public StocksClient(HttpClient httpClient, IConfiguration configuration, IMemoryCache memoryCache, ILogger<StocksClient> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<StockPriceResponse?> GetDataForTicker(string ticker)
    {
        _logger.LogInformation("Getting stock price information for {ticker}", ticker);

        StockPriceResponse? stockPriceResponse = await _memoryCache.GetOrCreateAsync($"stocks-{ticker}", async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            return await GetStockPrice(ticker);
        });

        if (stockPriceResponse is null)
        {
            _logger.LogWarning("Failed to get stock price information for {ticker}", ticker);
        }
        else
        {
            _logger.LogInformation(
                "Got stock price information for {ticker}: {@Stock}",
                ticker,
                stockPriceResponse
            );
        }

        return stockPriceResponse;
    }

    private async Task<StockPriceResponse?> GetStockPrice(string ticker)
    {
        string apiKey = _configuration["Stocks:ApiKey"]!;
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={ticker}" + $"&interval=5min&apikey={apiKey}";
        
        string tickerDataString = await _httpClient.GetStringAsync(url);
        AlphaVantageData? tickerData = JsonConvert.DeserializeObject<AlphaVantageData>(tickerDataString);

        TimeSeriesEntry? lastPrice = tickerData?.TimeSeries?.LastOrDefault().Value;

        if (lastPrice is null)
        {
            return null;
        }
        return new StockPriceResponse(ticker, decimal.Parse(lastPrice.High, CultureInfo.InvariantCulture), DateTime.Now);
    }
}
