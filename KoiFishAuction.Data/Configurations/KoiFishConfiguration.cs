using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class KoiFishConfiguration : IEntityTypeConfiguration<KoiFish>
    {
        public void Configure(EntityTypeBuilder<KoiFish> builder)
        {
            // Đặt tên bảng
            builder.ToTable("KoiFishes");

            // Khóa chính
            builder.HasKey(k => k.Id);

            // Các thuộc tính
            builder.Property(k => k.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(k => k.Description)
                   .HasMaxLength(1000);

            builder.Property(k => k.StartingPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(k => k.CurrentPrice)
                   .HasColumnType("decimal(18,2)");

            builder.Property(k => k.ImageUrl)
                   .HasMaxLength(500);

            builder.Property(k => k.Origin)
                   .HasMaxLength(100);

            builder.Property(k => k.ColorPattern)
                   .HasMaxLength(100);

            builder.Property(k => k.Weight)
                   .HasColumnType("decimal(18,2)");

            builder.Property(k => k.Length)
                   .HasColumnType("decimal(18,2)");

            builder.Property(k => k.Age)
                   .IsRequired();

            // Các mối quan hệ

            // N - 1: KoiFish - Seller (User)
            builder.HasOne(k => k.Seller)
                   .WithMany(u => u.KoiFishes)
                   .HasForeignKey(k => k.SellerId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1 - N: KoiFish - AuctionSessions
            builder.HasMany(k => k.AuctionSessions)
                   .WithOne(a => a.KoiFish)
                   .HasForeignKey(a => a.KoiFishId)
                   .OnDelete(DeleteBehavior.Restrict);

            // 1 - N: KoiFish - Notifications
            builder.HasMany(k => k.Notifications)
                   .WithOne(n => n.Item)
                   .HasForeignKey(n => n.ItemId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
