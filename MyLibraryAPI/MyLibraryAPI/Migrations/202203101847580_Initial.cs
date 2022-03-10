namespace MyLibraryAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditUsers",
                c => new
                    {
                        AuditUserId = c.Int(nullable: false, identity: true),
                        ExecutorUserId = c.Int(nullable: false),
                        AfectedUserId = c.Int(nullable: false),
                        Action = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AuditUserId)
                .ForeignKey("dbo.Users", t => t.AfectedUserId)
                .ForeignKey("dbo.Users", t => t.ExecutorUserId)
                .Index(t => t.ExecutorUserId)
                .Index(t => t.AfectedUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 150),
                        LastName = c.String(maxLength: 300),
                        Email = c.String(nullable: false, maxLength: 500),
                        Password = c.String(),
                        Active = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.Email, unique: true, name: "Unq_UsrEmail")
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.RoleId)
                .Index(t => t.Description, unique: true);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        PublishedYear = c.Int(nullable: false),
                        Genre = c.String(),
                        CreatedBy = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedBy = c.Int(),
                        UpdatedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Users", t => t.CreatedBy)
                .ForeignKey("dbo.Users", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Available = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.Books", t => t.StockId)
                .Index(t => t.StockId);
            
            CreateTable(
                "dbo.CheckOuts",
                c => new
                    {
                        CheckOutId = c.Int(nullable: false, identity: true),
                        StudentId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                        Returned = c.Boolean(nullable: false),
                        CheckedOutMoment = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CheckOutId)
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.Users", t => t.StudentId)
                .Index(t => t.StudentId)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CheckOuts", "StudentId", "dbo.Users");
            DropForeignKey("dbo.CheckOuts", "BookId", "dbo.Books");
            DropForeignKey("dbo.Stocks", "StockId", "dbo.Books");
            DropForeignKey("dbo.Books", "UpdatedBy", "dbo.Users");
            DropForeignKey("dbo.Books", "CreatedBy", "dbo.Users");
            DropForeignKey("dbo.AuditUsers", "ExecutorUserId", "dbo.Users");
            DropForeignKey("dbo.AuditUsers", "AfectedUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.CheckOuts", new[] { "BookId" });
            DropIndex("dbo.CheckOuts", new[] { "StudentId" });
            DropIndex("dbo.Stocks", new[] { "StockId" });
            DropIndex("dbo.Books", new[] { "UpdatedBy" });
            DropIndex("dbo.Books", new[] { "CreatedBy" });
            DropIndex("dbo.Roles", new[] { "Description" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Users", "Unq_UsrEmail");
            DropIndex("dbo.AuditUsers", new[] { "AfectedUserId" });
            DropIndex("dbo.AuditUsers", new[] { "ExecutorUserId" });
            DropTable("dbo.CheckOuts");
            DropTable("dbo.Stocks");
            DropTable("dbo.Books");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.AuditUsers");
        }
    }
}
