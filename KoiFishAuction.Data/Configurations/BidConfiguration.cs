using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            // Table name
            builder.ToTable("Bids");

            // Primary Key
            builder.HasKey(b => b.Id);

            // Properties
            builder.Property(b => b.Note)
                   .HasMaxLength(500);

            builder.Property(b => b.Currency)
                   .HasMaxLength(10);

            builder.Property(b => b.Location)
                   .HasMaxLength(100);

            // Relationships

            // N - 1: Bid - User (Bidder)
            builder.HasOne(b => b.Bidder)
                   .WithMany(u => u.Bids)
                   .HasForeignKey(b => b.BidderId)
                   .OnDelete(DeleteBehavior.Restrict);

            // N - 1: Bid - AuctionSession
            builder.HasOne(b => b.AuctionSession)
                   .WithMany(a => a.Bids)
                   .HasForeignKey(b => b.AuctionSessionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 1 - N: Bid - Notifications
            builder.HasMany(b => b.Notifications)
                   .WithOne(n => n.Bid)
                   .HasForeignKey(n => n.BidId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
