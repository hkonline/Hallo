using System;
using System.Linq;
using System.Data.Entity;
using HalloDal.Models.Content;
using HalloDal.Models.Users;
using System.Data.SqlClient;

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

        public User GetUserById(int id) {
            return Users.FirstOrDefault(x => x.UserId == id);
        }

        public void UpdateOrInsertUser(User user) {
            string sql = "insert into Users(";
            
            if (this.Users.Where(x => x.UserId == user.UserId).Count() > 0) { 
            
            }
                
            
            SqlCommand cmd = new SqlCommand("", this.Database.Connection as SqlConnection);



        } 


    }
}
