using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Hallo.Models.Mapping
{
    public class ImageMap : EntityTypeConfiguration<Image>
    {
        public ImageMap()
        {
            // Primary Key
            this.HasKey(t => t.ImageID);

            // Properties
            this.Property(t => t.ImageID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Description)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Images", "kobenhavn");
            this.Property(t => t.ImageID).HasColumnName("ImageID");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.OrderNr).HasColumnName("OrderNr");
        }
    }
}
