namespace HalloDal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Roles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.RoleUsers",
                c => new
                    {
                        Role_RoleId = c.Int(nullable: false),
                        User_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_RoleId, t.User_UserId })
                .ForeignKey("dbo.Roles", t => t.Role_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Role_RoleId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Users", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserName", c => c.String());
            DropPrimaryKey("dbo.Users", new[] { "PmoId" });
            AddPrimaryKey("dbo.Users", "UserId");
            DropColumn("dbo.Users", "PmoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PmoId", c => c.Int(nullable: false));
            DropIndex("dbo.RoleUsers", new[] { "User_UserId" });
            DropIndex("dbo.RoleUsers", new[] { "Role_RoleId" });
            DropForeignKey("dbo.RoleUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.RoleUsers", "Role_RoleId", "dbo.Roles");
            DropPrimaryKey("dbo.Users", new[] { "UserId" });
            AddPrimaryKey("dbo.Users", "PmoId");
            DropColumn("dbo.Users", "UserName");
            DropColumn("dbo.Users", "UserId");
            DropTable("dbo.RoleUsers");
            DropTable("dbo.Roles");
        }
    }
}
