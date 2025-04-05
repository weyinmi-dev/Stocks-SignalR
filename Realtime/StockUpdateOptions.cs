namespace Stocks_SignalR.Realtime;

public sealed class StockUpdateOptions
{
    public TimeSpan UpdateInterval { get; set; } = TimeSpan.FromSeconds(5);
    public double MaxPercentageChange { get; set; } = 0.02;
}
