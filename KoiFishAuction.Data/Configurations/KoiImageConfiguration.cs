using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KoiFishAuction.Data.Models;

namespace KoiFishAuction.Data.Configurations
{
    public class KoiImageConfiguration : IEntityTypeConfiguration<KoiImage>
    {
        public void Configure(EntityTypeBuilder<KoiImage> builder)
        {
            // Table name
            builder.ToTable("KoiImages");

            // Primary key
            builder.HasKey(ki => ki.Id);

            // Properties
            builder.Property(ki => ki.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            // Relationships
            builder.HasOne(ki => ki.KoiFish)
                   .WithMany(kf => kf.KoiImages)
                   .HasForeignKey(ki => ki.KoiFishId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
