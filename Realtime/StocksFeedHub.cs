using Microsoft.AspNetCore.SignalR;

namespace Stocks_SignalR.Realtime;

public class StocksFeedHub : Hub<IStockUpdateClient>
{
    public async Task JoinStockGroup(string ticker)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, ticker);
    }
}
