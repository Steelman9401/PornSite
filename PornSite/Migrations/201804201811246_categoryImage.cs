namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Img", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "Img");
        }
    }
}
