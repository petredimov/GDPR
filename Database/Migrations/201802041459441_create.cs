namespace DatabaseNamespace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Action = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserID = c.Int(),
                        ManagerID = c.Int(),
                        InviteDate = c.DateTime(nullable: false),
                        SigningDate = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        ContractTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ManagerID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.ContractTypes", t => t.ContractTypeId, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ManagerID)
                .Index(t => t.ContractTypeId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Address = c.String(),
                        CompanyName = c.String(),
                        Email = c.String(nullable: false),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.RoleId)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContractTypes",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Data = c.Binary(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contracts", "ContractTypeId", "dbo.ContractTypes");
            DropForeignKey("dbo.Users", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Contracts", "UserID", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Contracts", "ManagerID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "User_Id" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.Contracts", new[] { "ContractTypeId" });
            DropIndex("dbo.Contracts", new[] { "ManagerID" });
            DropIndex("dbo.Contracts", new[] { "UserID" });
            DropTable("dbo.ContractTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.Contracts");
            DropTable("dbo.Audits");
        }
    }
}
