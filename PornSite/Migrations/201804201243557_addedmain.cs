namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedmain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "AllowMain", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "AllowMain");
        }
    }
}
