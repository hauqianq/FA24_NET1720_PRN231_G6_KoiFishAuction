using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(u => u.FullName)
                   .HasMaxLength(200);

            builder.Property(u => u.PhoneNumber)
                   .HasMaxLength(50);

            builder.Property(u => u.Address)
                   .HasMaxLength(500);

            // Relationships

            // 1 - N: User - Bids
            builder.HasMany(u => u.Bids)
                   .WithOne(b => b.Bidder)
                   .HasForeignKey(b => b.BidderId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - KoiFishes
            builder.HasMany(u => u.KoiFishes)
                   .WithOne(k => k.Seller)
                   .HasForeignKey(k => k.SellerId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - AuctionHistory (Owner)
            builder.HasMany(u => u.AuctionHistoryOwners)
                   .WithOne(ah => ah.Owner)
                   .HasForeignKey(ah => ah.OwnerId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - AuctionHistory (Winner)
            builder.HasMany(u => u.AuctionHistoryWinners)
                   .WithOne(ah => ah.Winner)
                   .HasForeignKey(ah => ah.WinnerId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - Notifications (Sender)
            builder.HasMany(u => u.NotificationSenders)
                   .WithOne(n => n.Sender)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - Notifications (Recipient)
            builder.HasMany(u => u.NotificationUsers)
                   .WithOne(n => n.User)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            // 1 - N: User - Payments
            builder.HasMany(u => u.Payments)
                   .WithOne(p => p.User)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
