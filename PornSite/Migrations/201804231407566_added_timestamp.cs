namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_timestamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Videos", "TimeStamp", c => c.Long(nullable: false));
            DropColumn("dbo.Videos", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Videos", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Videos", "TimeStamp");
        }
    }
}
