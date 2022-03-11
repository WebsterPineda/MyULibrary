namespace MyLibraryAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTempPass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "TempPassword", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TempPassword");
        }
    }
}
