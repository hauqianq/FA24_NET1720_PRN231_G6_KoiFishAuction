using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Đặt tên bảng
            builder.ToTable("Payments");

            // Khóa chính
            builder.HasKey(p => p.Id);

            // Các thuộc tính
            builder.Property(p => p.Method)
                   .HasMaxLength(50);

            builder.Property(p => p.Status)
                   .HasMaxLength(50);

            builder.Property(p => p.Confirmation)
                   .HasMaxLength(100);

            builder.Property(p => p.Remarks)
                   .HasMaxLength(500);

            builder.Property(p => p.Currency)
                   .HasMaxLength(10);

            builder.Property(p => p.Amount)
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Date)
                   .IsRequired();

            // Các mối quan hệ

            // N - 1: Payment - AuctionHistory
            builder.HasOne(p => p.History)
                   .WithMany(ah => ah.Payments)
                   .HasForeignKey(p => p.HistoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // N - 1: Payment - User
            builder.HasOne(p => p.User)
                   .WithMany(u => u.Payments)
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
