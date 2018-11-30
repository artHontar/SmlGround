namespace SmlGround.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dialogs",
                c => new
                    {
                        DialogId = c.Long(nullable: false, identity: true),
                        UserOneId = c.String(nullable: false, maxLength: 128),
                        UserTwoId = c.String(maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DialogId)
                .ForeignKey("dbo.Users", t => t.UserOneId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserTwoId)
                .Index(t => t.UserOneId)
                .Index(t => t.UserTwoId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MyId = c.Long(nullable: false, identity: true),
                        DialogId = c.Long(),
                        Text = c.String(nullable: false, maxLength: 255),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MyId)
                .ForeignKey("dbo.Dialogs", t => t.DialogId)
                .Index(t => t.DialogId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RegistrationTime = c.DateTime(nullable: false),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Long(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Image = c.Binary(),
                        Text = c.String(maxLength: 255),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Surname = c.String(),
                        Avatar = c.Binary(),
                        Birthday = c.DateTime(),
                        City = c.String(maxLength: 54),
                        PlaceOfStudy = c.String(maxLength: 128),
                        Skype = c.String(maxLength: 54),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        UserById = c.String(nullable: false, maxLength: 128),
                        UserToId = c.String(nullable: false, maxLength: 128),
                        CreationTime = c.DateTime(nullable: false),
                        FriendRequestFlag = c.Int(nullable: false),
                        Relationship = c.String(maxLength: 54),
                    })
                .PrimaryKey(t => new { t.UserById, t.UserToId })
                .ForeignKey("dbo.Users", t => t.UserById)
                .ForeignKey("dbo.Users", t => t.UserToId)
                .Index(t => t.UserById)
                .Index(t => t.UserToId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Dialogs", "UserTwoId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Friends", "UserToId", "dbo.Users");
            DropForeignKey("dbo.Friends", "UserById", "dbo.Users");
            DropForeignKey("dbo.Profiles", "Id", "dbo.Users");
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Dialogs", "UserOneId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "DialogId", "dbo.Dialogs");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserToId" });
            DropIndex("dbo.Friends", new[] { "UserById" });
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "DialogId" });
            DropIndex("dbo.Dialogs", new[] { "UserTwoId" });
            DropIndex("dbo.Dialogs", new[] { "UserOneId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Friends");
            DropTable("dbo.Profiles");
            DropTable("dbo.Posts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.Dialogs");
        }
    }
}
