using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Hallo.Models.Mapping;

namespace Hallo.Models
{
    public partial class kobenhavnContext : DbContext
    {
        static kobenhavnContext()
        {
            Database.SetInitializer<kobenhavnContext>(null);
        }

        public kobenhavnContext()
            : base("Name=kobenhavnContext")
        {
        }

        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<InfoBox> InfoBoxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new ArticleMap());
            modelBuilder.Configurations.Add(new ImageMap());
            modelBuilder.Configurations.Add(new InfoBoxMap());
        }
    }
}
