using System.Collections.Concurrent;

namespace Stocks_SignalR.Realtime;

public sealed class ActiveTickerManager
{
    private readonly ConcurrentBag<string> _activeTickers = [];

    public void AddTicker(string ticker)
    {
        _activeTickers.Add(ticker);
    }
    public IReadOnlyCollection<string> GetAllTickers()
    {
        return _activeTickers.ToArray();
    }
}
