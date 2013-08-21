using System.Data.Entity;
using HalloDal.Models.Content;
using HalloDal.Models.Users;

namespace HalloDal.Models {
    public partial class HalloContext : DbContext {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ArticleCategory> Categories { get; set; }
    }
}
