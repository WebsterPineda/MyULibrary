namespace MyLibraryAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Privileges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Privileges",
                c => new
                    {
                        PrivilegeId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        Controller = c.String(nullable: false, maxLength: 125),
                        Action = c.String(nullable: false, maxLength: 125),
                        Granted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PrivilegeId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            AlterColumn("dbo.Books", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Author", c => c.String(nullable: false));
            AlterColumn("dbo.Books", "Genre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Privileges", "RoleId", "dbo.Roles");
            DropIndex("dbo.Privileges", new[] { "RoleId" });
            AlterColumn("dbo.Books", "Genre", c => c.String());
            AlterColumn("dbo.Books", "Author", c => c.String());
            AlterColumn("dbo.Books", "Title", c => c.String());
            DropTable("dbo.Privileges");
        }
    }
}
