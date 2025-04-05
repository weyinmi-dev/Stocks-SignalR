using Microsoft.EntityFrameworkCore;
using Stocks_SignalR.Stocks;

namespace Stocks_SignalR.Infrastructure
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
      
        public DbSet<StockPriceResponse>? StockPrices { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockPriceResponse>().HasKey(s => s.Ticker);
            modelBuilder.Entity<StockPriceResponse>().Property(s => s.Timestamp).IsRequired(); // Add this line
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=MELIOS-IT-2;Database=StocksFeed;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
    }
} 