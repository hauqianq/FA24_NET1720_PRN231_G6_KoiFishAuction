using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class AuctionSessionConfiguration : IEntityTypeConfiguration<AuctionSession>
    {
        public void Configure(EntityTypeBuilder<AuctionSession> builder)
        {
            // Đặt tên bảng
            builder.ToTable("AuctionSessions");

            // Khóa chính
            builder.HasKey(a => a.Id);

            // Các thuộc tính
            builder.Property(a => a.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.Note)
                   .HasMaxLength(500);

            builder.Property(a => a.Status)
                   .IsRequired();

            builder.Property(a => a.MinIncrement)
                   .HasColumnType("decimal(18,2)");

            builder.Property(a => a.StartTime)
                   .IsRequired();

            builder.Property(a => a.EndTime)
                   .IsRequired();

            // Các mối quan hệ

            // N - 1: AuctionSession - KoiFish
            builder.HasOne(a => a.KoiFish)
                   .WithMany(k => k.AuctionSessions)
                   .HasForeignKey(a => a.KoiFishId)
                   .OnDelete(DeleteBehavior.Restrict);

            // N - 1: AuctionSession - Creator (User)
            builder.HasOne(a => a.Creator)
                   .WithMany()
                   .HasForeignKey(a => a.CreatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1 - 1: AuctionSession - AuctionHistory
            builder.HasOne(a => a.AuctionHistory)
                   .WithOne(ah => ah.AuctionSession)
                   .HasForeignKey<AuctionHistory>(ah => ah.AuctionSessionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // 1 - N: AuctionSession - Bids
            builder.HasMany(a => a.Bids)
                   .WithOne(b => b.AuctionSession)
                   .HasForeignKey(b => b.AuctionSessionId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
