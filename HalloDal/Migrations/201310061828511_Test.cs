namespace HalloDal.Migrations {
    using System;
    using System.Data.Entity.Migrations;

    public partial class Test : DbMigration {

        public override void Up() {
            DropForeignKey("dbo.ArticleCategories", "Article_Id", "dbo.Articles");
            DropIndex("dbo.ArticleCategories", new[] { "Article_Id" });
            CreateTable(
                "dbo.Roles",
                c => new {
                    RoleId = c.Int(nullable: false, identity: true),
                    RoleName = c.String(),
                    Description = c.String(),
                })
                .PrimaryKey(t => t.RoleId);

            CreateTable(
                "dbo.LogEntries",
                c => new {
                    LogEntryId = c.Long(nullable: false, identity: true),
                    PmoId = c.Int(nullable: false),
                    UserName = c.String(maxLength: 20),
                    FirstName = c.String(maxLength: 40),
                    LastName = c.String(maxLength: 40),
                    Gender = c.String(maxLength: 1),
                    Country = c.String(maxLength: 50),
                    ChurchId = c.Int(nullable: false),
                    ChurchName = c.String(maxLength: 50),
                    HomeChurchId = c.Int(nullable: false),
                    HomeChurchName = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.LogEntryId);

            CreateTable(
                "dbo.ArticleCategoryArticles",
                c => new {
                    ArticleCategory_Id = c.Int(nullable: false),
                    Article_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.ArticleCategory_Id, t.Article_Id })
                .ForeignKey("dbo.ArticleCategories", t => t.ArticleCategory_Id, cascadeDelete: true)
                .ForeignKey("dbo.Articles", t => t.Article_Id, cascadeDelete: true)
                .Index(t => t.ArticleCategory_Id)
                .Index(t => t.Article_Id);

            CreateTable(
                "dbo.RoleUsers",
                c => new {
                    Role_RoleId = c.Int(nullable: false),
                    User_UserId = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.Role_RoleId, t.User_UserId })
                .ForeignKey("dbo.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId);

            AddColumn("dbo.Users", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserName", c => c.String(maxLength: 20));
            AddColumn("dbo.Users", "Country", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "ChurchId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "ChurchName", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "HomeChurchId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "HomeChurchName", c => c.String(maxLength: 50));
            AddColumn("dbo.Users", "Authorized", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "NumberOfVisits", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "LastVisit", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "Firstname", c => c.String(maxLength: 40));
            AlterColumn("dbo.Users", "Lastname", c => c.String(maxLength: 40));
            AlterColumn("dbo.Users", "MobilPhone", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.Users", "LocalBccAccount", c => c.String(maxLength: 20));
            AlterColumn("dbo.Users", "Gender", c => c.String(maxLength: 1));
            DropPrimaryKey("dbo.Users", new[] { "PmoId" });
            AddPrimaryKey("dbo.Users", "UserId");
            DropColumn("dbo.ArticleCategories", "Article_Id");
            DropColumn("dbo.Users", "PmoId");
            DropColumn("dbo.Users", "LiveInLocalChurch");
            DropColumn("dbo.Users", "AttendsMeetings");
        }

        public override void Down() {
            AddColumn("dbo.Users", "AttendsMeetings", c => c.Boolean());
            AddColumn("dbo.Users", "LiveInLocalChurch", c => c.Boolean());
            AddColumn("dbo.Users", "PmoId", c => c.Int(nullable: false));
            AddColumn("dbo.ArticleCategories", "Article_Id", c => c.Int());
            DropIndex("dbo.RoleUsers", new[] { "User_UserId" });
            DropIndex("dbo.RoleUsers", new[] { "Role_RoleId" });
            DropIndex("dbo.ArticleCategoryArticles", new[] { "Article_Id" });
            DropIndex("dbo.ArticleCategoryArticles", new[] { "ArticleCategory_Id" });
            DropForeignKey("dbo.RoleUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_RoleId", "dbo.Roles");
            DropForeignKey("dbo.ArticleCategoryArticles", "Article_Id", "dbo.Articles");
            DropForeignKey("dbo.ArticleCategoryArticles", "ArticleCategory_Id", "dbo.ArticleCategories");
            DropPrimaryKey("dbo.Users", new[] { "UserId" });
            AddPrimaryKey("dbo.Users", "PmoId");
            AlterColumn("dbo.Users", "Gender", c => c.String());
            AlterColumn("dbo.Users", "LocalBccAccount", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "MobilPhone", c => c.String());
            AlterColumn("dbo.Users", "Lastname", c => c.String());
            AlterColumn("dbo.Users", "Firstname", c => c.String());
            DropColumn("dbo.Users", "LastVisit");
            DropColumn("dbo.Users", "NumberOfVisits");
            DropColumn("dbo.Users", "Authorized");
            DropColumn("dbo.Users", "HomeChurchName");
            DropColumn("dbo.Users", "HomeChurchId");
            DropColumn("dbo.Users", "ChurchName");
            DropColumn("dbo.Users", "ChurchId");
            DropColumn("dbo.Users", "Country");
            DropColumn("dbo.Users", "UserName");
            DropColumn("dbo.Users", "UserId");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.ArticleCategoryArticles");
            DropTable("dbo.LogEntries");
            DropTable("dbo.Roles");
            CreateIndex("dbo.ArticleCategories", "Article_Id");
            AddForeignKey("dbo.ArticleCategories", "Article_Id", "dbo.Articles", "Id");
        }
    }
}
