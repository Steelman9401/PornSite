namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class global : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Name_en", c => c.String());
            AddColumn("dbo.Videos", "Title_en", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Videos", "Title_en");
            DropColumn("dbo.Categories", "Name_en");
        }
    }
}
