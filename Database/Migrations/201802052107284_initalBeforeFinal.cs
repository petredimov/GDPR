namespace DatabaseNamespace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initalBeforeFinal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MailConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Username = c.String(),
                        Password = c.String(),
                        Port = c.Int(nullable: false),
                        EnableSsl = c.Boolean(nullable: false),
                        Host = c.String(),
                        MailType = c.Int(nullable: false),
                        From = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MailConfigurations", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.MailConfigurations", new[] { "UserId" });
            DropTable("dbo.MailConfigurations");
        }
    }
}
