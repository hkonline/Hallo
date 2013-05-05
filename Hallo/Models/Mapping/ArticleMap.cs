using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Hallo.Models.Mapping
{
    public class ArticleMap : EntityTypeConfiguration<Article>
    {
        public ArticleMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ArtikelID, t.Overskrift, t.PublicArticle, t.IsSlideshow, t.ArticleType });

            // Properties
            this.Property(t => t.ArtikelID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Overskrift)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Forfatter)
                .HasMaxLength(50);

            this.Property(t => t.URL)
                .HasMaxLength(60);

            this.Property(t => t.ForsideBilledeURL)
                .HasMaxLength(50);

            this.Property(t => t.ArticleType)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Articles", "kobenhavn");
            this.Property(t => t.ArtikelID).HasColumnName("ArtikelID");
            this.Property(t => t.Overskrift).HasColumnName("Overskrift");
            this.Property(t => t.Forfatter).HasColumnName("Forfatter");
            this.Property(t => t.Dato).HasColumnName("Dato");
            this.Property(t => t.URL).HasColumnName("URL");
            this.Property(t => t.ForsideTekst).HasColumnName("ForsideTekst");
            this.Property(t => t.ForsideBilledeURL).HasColumnName("ForsideBilledeURL");
            this.Property(t => t.ForsideBilledeID).HasColumnName("ForsideBilledeID");
            this.Property(t => t.Tekst).HasColumnName("Tekst");
            this.Property(t => t.HarKant).HasColumnName("HarKant");
            this.Property(t => t.ErAutomatiseret).HasColumnName("ErAutomatiseret");
            this.Property(t => t.Layout).HasColumnName("Layout");
            this.Property(t => t.IsCheckedByJens).HasColumnName("IsCheckedByJens");
            this.Property(t => t.PublicArticle).HasColumnName("PublicArticle");
            this.Property(t => t.IsSlideshow).HasColumnName("IsSlideshow");
            this.Property(t => t.ArticleType).HasColumnName("ArticleType");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Category2).HasColumnName("Category2");
        }
    }
}
