using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class AuctionHistoryConfiguration : IEntityTypeConfiguration<AuctionHistory>
    {
        public void Configure(EntityTypeBuilder<AuctionHistory> builder)
        {
            // Đặt tên bảng
            builder.ToTable("AuctionHistories");

            // Khóa chính
            builder.HasKey(ah => ah.Id);

            // Các thuộc tính
            builder.Property(ah => ah.PaymentStatus)
                   .HasMaxLength(50);

            builder.Property(ah => ah.DeliveryStatus)
                   .HasMaxLength(50);

            builder.Property(ah => ah.FeedbackStatus)
                   .HasMaxLength(50);

            builder.Property(ah => ah.Remarks)
                   .HasMaxLength(500);

            builder.Property(ah => ah.WinningAmount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(ah => ah.WinningDate)
                   .IsRequired();

            // Các mối quan hệ

            // 1 - 1: AuctionHistory - AuctionSession
            builder.HasOne(ah => ah.AuctionSession)
                   .WithOne(a => a.AuctionHistory)
                   .HasForeignKey<AuctionHistory>(ah => ah.AuctionSessionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // N - 1: AuctionHistory - Owner (User)
            builder.HasOne(ah => ah.Owner)
                   .WithMany(u => u.AuctionHistoryOwners)
                   .HasForeignKey(ah => ah.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // N - 1: AuctionHistory - Winner (User)
            builder.HasOne(ah => ah.Winner)
                   .WithMany(u => u.AuctionHistoryWinners)
                   .HasForeignKey(ah => ah.WinnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1 - N: AuctionHistory - Payments
            builder.HasMany(ah => ah.Payments)
                   .WithOne(p => p.History)
                   .HasForeignKey(p => p.HistoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
