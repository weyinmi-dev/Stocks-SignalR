using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stocks_SignalR.Realtime;
using Stocks_SignalR.Infrastructure;

namespace Stocks_SignalR.Stocks
{
    public class StockService
    {
        private readonly AppDbContext _context;
        private readonly StocksClient _stocksClient;
        private readonly ActiveTickerManager _activeTickerManager;
        private readonly ILogger<StockService> _logger;

        public StockService(AppDbContext context, StocksClient stocksClient, ActiveTickerManager activeTickerManager, ILogger<StockService> logger)
        {
            _context = context;
            _stocksClient = stocksClient;
            _activeTickerManager = activeTickerManager;
            _logger = logger;
        }

        public async Task<StockPriceResponse?> GetLatestStockPrice(string ticker)
        {
            try
            {
                // First, try to get the latest price from the database
                StockPriceResponse? dbPrice = await GetLatestPriceFromDatabase(ticker);
                if (dbPrice is not null)
                {
                    _activeTickerManager.AddTicker(ticker);
                    return dbPrice;
                }

                // If not found in the database, fetch from the external API
                StockPriceResponse? apiPrice = await _stocksClient.GetDataForTicker(ticker);

                if (apiPrice == null)
                {
                    _logger.LogWarning("No data returned from external API for ticker: {Ticker}", ticker);
                    return null;
                }

                // Save the new price to the database
                await SavePriceToDatabase(apiPrice);

                _activeTickerManager.AddTicker(ticker);

                return apiPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching stock price for ticker: {Ticker}", ticker);
                throw;
            }
        }

        private async Task<StockPriceResponse?> GetLatestPriceFromDatabase(string ticker)
        {
            return await _context.StockPrices
                .Where(s => s.Ticker == ticker)
                .OrderByDescending(s => EF.Property<DateTime>(s, "Timestamp"))
                .FirstOrDefaultAsync();
        }

        private async Task SavePriceToDatabase(StockPriceResponse price)
        {
            _context.StockPrices.Add(price);
            await _context.SaveChangesAsync();
        }
    }
}
