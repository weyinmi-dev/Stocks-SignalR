namespace Stocks_SignalR.Realtime
{
    public interface IStockUpdateClient
    {
        Task ReceiveStockPriceUpdate(StockPriceUpdate update);
    }
}
