using KoiFishAuction.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KoiFishAuction.Data.Models;

public partial class FA24_NET1720_PRN231_G6_KoiFishAuctionContext : DbContext
{
    public FA24_NET1720_PRN231_G6_KoiFishAuctionContext()
    {

    }
    public FA24_NET1720_PRN231_G6_KoiFishAuctionContext(DbContextOptions<FA24_NET1720_PRN231_G6_KoiFishAuctionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuctionHistory> AuctionHistories { get; set; }

    public virtual DbSet<AuctionSession> AuctionSessions { get; set; }

    public virtual DbSet<Bid> Bids { get; set; }

    public virtual DbSet<KoiFish> KoiFishes { get; set; }

    public virtual DbSet<KoiImage> KoiImages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string root = Directory.GetParent(Directory.GetCurrentDirectory())?.FullName;
        string apiDirectory = Path.Combine(root, "KoiFishAuction.API");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiDirectory)
            .AddJsonFile("appsettings.Development.json")
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FA24_NET1720_PRN231_G6_KoiFishAuctionContext).Assembly);
        modelBuilder.SeedingData();
    }
}