namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "User_Id", c => c.Int());
            CreateIndex("dbo.Videos", "User_Id");
            AddForeignKey("dbo.Videos", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Videos", "User_Id", "dbo.Users");
            DropIndex("dbo.Videos", new[] { "User_Id" });
            DropColumn("dbo.Videos", "User_Id");
        }
    }
}
