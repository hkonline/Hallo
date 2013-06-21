using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Hallo.Models.Mapping
{
    public class InfoBoxMap : EntityTypeConfiguration<InfoBox>
    {
        public InfoBoxMap()
        {
            // Primary Key
            this.HasKey(t => new { t.id, t.isPublic });

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.overskrift)
                .HasMaxLength(80);

            this.Property(t => t.forfatter)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("InfoBox", "kobenhavn");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.overskrift).HasColumnName("overskrift");
            this.Property(t => t.forfatter).HasColumnName("forfatter");
            this.Property(t => t.info).HasColumnName("info");
            this.Property(t => t.dato_begynd).HasColumnName("dato_begynd");
            this.Property(t => t.dato_slut).HasColumnName("dato_slut");
            this.Property(t => t.isPublic).HasColumnName("isPublic");
        }
    }
}
