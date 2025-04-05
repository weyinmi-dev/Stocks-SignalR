using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Stocks_SignalR.Stocks;

public class AlphaVantageData
{
    [JsonProperty("Meta Data")]
    public MetaData MetaData { get; set; } = null!;

    [JsonProperty("Time Series (5min)")]
    public Dictionary<string, TimeSeriesEntry> TimeSeries { get; set; } = null!;
}

public class MetaData
{
    [JsonProperty("1. Information")]
    public string Information { get; set; } = null!;

    [JsonProperty("2. Symbol")]
    public string Symbol { get; set; } = null!;

    [JsonProperty("3. Last Refreshed")]
    public string LastRefreshed { get; set; } = null!;

    [JsonProperty("4. Interval")]
    public string Interval { get; set; } = null!;

    [JsonProperty("5. Output Size")]
    public string OutputSize { get; set; } = null!;

    [JsonProperty("6. Time Zone")]
    public string TimeZone { get; set; } = null!;
}

public class TimeSeriesEntry
{
    [JsonProperty("1. open")]
    public string Open { get; set; } = null!;

    [JsonProperty("2. high")]
    public string High { get; set; } = null!;

    [JsonProperty("3. low")]
    public string Low { get; set; } = null!;

    [JsonProperty("4. close")]
    public string Close { get; set; } = null!;

    [JsonProperty("5. volume")]
    public string Volume { get; set; } = null!;
}