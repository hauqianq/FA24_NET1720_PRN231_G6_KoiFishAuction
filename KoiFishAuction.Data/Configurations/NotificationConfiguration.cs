using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Đặt tên bảng
            builder.ToTable("Notifications");

            // Khóa chính
            builder.HasKey(n => n.Id);

            // Các thuộc tính
            builder.Property(n => n.Message)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(n => n.Type)
                   .HasMaxLength(50);

            builder.Property(n => n.Remarks)
                   .HasMaxLength(500);

            builder.Property(n => n.IsRead)
                   .HasDefaultValue(false);

            builder.Property(n => n.Date)
                   .IsRequired();

            // Các mối quan hệ

            // N - 1: Notification - Sender (User)
            builder.HasOne(n => n.Sender)
                   .WithMany(u => u.NotificationSenders)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            // N - 1: Notification - Recipient (User)
            builder.HasOne(n => n.User)
                   .WithMany(u => u.NotificationUsers)
                   .HasForeignKey(n => n.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // N - 1: Notification - Bid
            builder.HasOne(n => n.Bid)
                   .WithMany(b => b.Notifications)
                   .HasForeignKey(n => n.BidId)
                   .OnDelete(DeleteBehavior.SetNull);

            // N - 1: Notification - KoiFish (Item)
            builder.HasOne(n => n.Item)
                   .WithMany(k => k.Notifications)
                   .HasForeignKey(n => n.ItemId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
