using System.Data.Entity;
using HalloDal.Models.Content;
using HalloDal.Models.Users;

namespace HalloDal.Models {
    public partial class HalloContext : DbContext {
        public DbSet<FrontpageLink> FrontpageLinks { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleCategory> Categories { get; set; }
        public DbSet<Image> Images { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
    }
}
