namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "User_Id", c => c.Int());
            CreateIndex("dbo.Categories", "User_Id");
            AddForeignKey("dbo.Categories", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "User_Id", "dbo.Users");
            DropIndex("dbo.Categories", new[] { "User_Id" });
            DropColumn("dbo.Categories", "User_Id");
        }
    }
}
