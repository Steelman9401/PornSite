namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class root : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Url = c.String(),
                        Description = c.String(),
                        Views = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Img = c.String(),
                        Preview = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        User_Id = c.Int(),
                        Video_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Videos", t => t.Video_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Video_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Admin = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VideoCategories",
                c => new
                    {
                        Video_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Video_Id, t.Category_Id })
                .ForeignKey("dbo.Videos", t => t.Video_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.Video_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Video_Id", "dbo.Videos");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.VideoCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.VideoCategories", "Video_Id", "dbo.Videos");
            DropIndex("dbo.VideoCategories", new[] { "Category_Id" });
            DropIndex("dbo.VideoCategories", new[] { "Video_Id" });
            DropIndex("dbo.Comments", new[] { "Video_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropTable("dbo.VideoCategories");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Videos");
            DropTable("dbo.Categories");
        }
    }
}
