namespace Stocks_SignalR.Stocks;

public sealed record StockPriceResponse
{
    public string Ticker { get; init; }
    public decimal Price { get; init; }
    public DateTime Timestamp { get; init; } // Add this property

    public StockPriceResponse(string ticker, decimal price, DateTime timestamp)
    {
        Ticker = ticker;
        Price = price;
        Timestamp = timestamp;
    }

    // Parameterless constructor for EF Core
    private StockPriceResponse() { }
}

