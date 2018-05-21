namespace PornSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Name_en = c.String(),
                        Img = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Title_en = c.String(),
                        Url = c.String(),
                        Description = c.String(),
                        Views = c.Int(nullable: false),
                        Duration = c.String(),
                        HD = c.Boolean(nullable: false),
                        TimeStamp = c.Long(nullable: false),
                        Img = c.String(),
                        AllowMain = c.Boolean(nullable: false),
                        Preview = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
            DropForeignKey("dbo.Videos", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Video_Id", "dbo.Videos");
            DropForeignKey("dbo.VideoCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.VideoCategories", "Video_Id", "dbo.Videos");
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Categories", "User_Id", "dbo.Users");
            DropIndex("dbo.VideoCategories", new[] { "Category_Id" });
            DropIndex("dbo.VideoCategories", new[] { "Video_Id" });
            DropIndex("dbo.Videos", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Video_Id" });
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Categories", new[] { "User_Id" });
            DropTable("dbo.VideoCategories");
            DropTable("dbo.Videos");
            DropTable("dbo.Comments");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
        }
    }
}
