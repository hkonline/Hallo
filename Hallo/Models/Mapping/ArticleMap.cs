using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Hallo.Models.Mapping
{
    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            // Primary Key
            this.HasKey(t => t.ArticleId);

            // Properties
            this.Property(t => t.ArticleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Headline)
                .HasMaxLength(200);

            this.Property(t => t.Author)
                .HasMaxLength(100);

            this.Property(t => t.FrontpageText)
                .HasMaxLength(4000);

            this.Property(t => t.ArticleType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Articles", "kobenhavn");
            this.Property(t => t.ArticleId).HasColumnName("ArticleId");
            this.Property(t => t.Headline).HasColumnName("Headline");
            this.Property(t => t.Author).HasColumnName("Author");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.FrontpageText).HasColumnName("FrontpageText");
            this.Property(t => t.FrontpageImageId).HasColumnName("FrontpageImageId");
            this.Property(t => t.Text).HasColumnName("Text");
            this.Property(t => t.ApprovedByEditor).HasColumnName("ApprovedByEditor");
            this.Property(t => t.IsPublic).HasColumnName("IsPublic");
            this.Property(t => t.ArticleType).HasColumnName("ArticleType");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Category2).HasColumnName("Category2");

            // Relationships
            this.HasOptional(t => t.Image)
                .WithMany(t => t.Articles)
                .HasForeignKey(d => d.FrontpageImageId);

        }
    }
}
