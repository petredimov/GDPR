namespace DatabaseNamespace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialthree : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateRequested = c.DateTime(nullable: false),
                        ValidUntil = c.DateTime(nullable: false),
                        Url = c.String(),
                        UserId = c.String(maxLength: 128),
                        ManagerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ManagerId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ManagerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Requests", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Requests", new[] { "ManagerId" });
            DropIndex("dbo.Requests", new[] { "UserId" });
            DropTable("dbo.Requests");
        }
    }
}
