using Microsoft.EntityFrameworkCore;
using Stocks_SignalR.Infrastructure;
using Stocks_SignalR.Realtime;
using Stocks_SignalR.Stocks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();

builder.Services.AddHttpClient<StocksClient>();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register AppDbContext with a connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StocksConnection")));

// Register StocksClient and StockService
builder.Services.AddScoped<StocksClient>();
builder.Services.AddScoped<StockService>();
builder.Services.AddSingleton<ActiveTickerManager>();
builder.Services.AddHostedService<StocksFeedUpdater>();
builder.Services.Configure<StockUpdateOptions>(builder.Configuration.GetSection("StockUpdateOptions"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors(policy => policy
        .WithOrigins(builder.Configuration["Cors:AllowedOrigins"]!)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    );
}

app.MapGet("/api/stocks/{ticker}", async (string ticker, StockService stockService) =>
{
    StockPriceResponse? stockPrice = await stockService.GetLatestStockPrice(ticker);
    if (stockPrice is null)
    {
        return Results.NotFound($"No stock data available for ticker: {ticker}");
    }
    return Results.Ok(stockPrice);
})
.WithName("GetLatestStockPrice")
.WithOpenApi();

app.MapHub<StocksFeedHub>("/stocksfeed")  ;

app.UseHttpsRedirection();

app.Run();
