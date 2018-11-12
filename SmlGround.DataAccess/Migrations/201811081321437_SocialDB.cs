namespace SmlGround.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dialogs",
                c => new
                    {
                        DialogId = c.Long(nullable: false, identity: true),
                        UserOneId = c.Long(),
                        UserTwoId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        UserOne_Id = c.String(maxLength: 128),
                        UserTwo_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.DialogId)
                .ForeignKey("dbo.Users", t => t.UserOne_Id)
                .ForeignKey("dbo.Users", t => t.UserTwo_Id)
                .Index(t => t.UserOne_Id)
                .Index(t => t.UserTwo_Id);
            
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
                "dbo.Friends",
                c => new
                    {
                        FriendId = c.Long(nullable: false, identity: true),
                        UserOneId = c.Long(),
                        UserTwoId = c.Long(),
                        CreationTime = c.DateTime(nullable: false),
                        Relationship = c.String(maxLength: 54),
                        UserTwo_Id = c.String(maxLength: 128),
                        UserOne_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FriendId)
                .ForeignKey("dbo.Users", t => t.UserTwo_Id)
                .ForeignKey("dbo.Users", t => t.UserOne_Id)
                .Index(t => t.UserTwo_Id)
                .Index(t => t.UserOne_Id);
            
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
                        UserId = c.Long(),
                        Image = c.Binary(),
                        Text = c.String(maxLength: 255),
                        CreationTime = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
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
            DropForeignKey("dbo.Dialogs", "UserTwo_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Profiles", "Id", "dbo.Users");
            DropForeignKey("dbo.Posts", "User_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Friends", "UserOne_Id", "dbo.Users");
            DropForeignKey("dbo.Friends", "UserTwo_Id", "dbo.Users");
            DropForeignKey("dbo.Dialogs", "UserOne_Id", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.Messages", "DialogId", "dbo.Dialogs");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropIndex("dbo.Posts", new[] { "User_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserOne_Id" });
            DropIndex("dbo.Friends", new[] { "UserTwo_Id" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Messages", new[] { "DialogId" });
            DropIndex("dbo.Dialogs", new[] { "UserTwo_Id" });
            DropIndex("dbo.Dialogs", new[] { "UserOne_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Profiles");
            DropTable("dbo.Posts");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Friends");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Messages");
            DropTable("dbo.Dialogs");
        }
    }
}
