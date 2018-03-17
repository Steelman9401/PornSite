namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class durHD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "Duration", c => c.String());
            AddColumn("dbo.Videos", "HD", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "HD");
            DropColumn("dbo.Videos", "Duration");
        }
    }
}
