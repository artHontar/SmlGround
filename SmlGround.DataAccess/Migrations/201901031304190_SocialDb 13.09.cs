namespace SmlGround.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialDb1309 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dialogs", "UserOneId", "dbo.Users");
            DropForeignKey("dbo.Dialogs", "UserTwoId", "dbo.Users");
            DropIndex("dbo.Dialogs", new[] { "UserOneId" });
            DropIndex("dbo.Dialogs", new[] { "UserTwoId" });
            CreateTable(
                "dbo.UserDialogs",
                c => new
                    {
                        User_Id = c.String(nullable: false, maxLength: 128),
                        Dialog_DialogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Dialog_DialogId })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Dialogs", t => t.Dialog_DialogId, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Dialog_DialogId);
            
            AddColumn("dbo.Messages", "Read", c => c.Boolean(nullable: false));
            DropColumn("dbo.Dialogs", "UserOneId");
            DropColumn("dbo.Dialogs", "UserTwoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dialogs", "UserTwoId", c => c.String(maxLength: 128));
            AddColumn("dbo.Dialogs", "UserOneId", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.UserDialogs", "Dialog_DialogId", "dbo.Dialogs");
            DropForeignKey("dbo.UserDialogs", "User_Id", "dbo.Users");
            DropIndex("dbo.UserDialogs", new[] { "Dialog_DialogId" });
            DropIndex("dbo.UserDialogs", new[] { "User_Id" });
            DropColumn("dbo.Messages", "Read");
            DropTable("dbo.UserDialogs");
            CreateIndex("dbo.Dialogs", "UserTwoId");
            CreateIndex("dbo.Dialogs", "UserOneId");
            AddForeignKey("dbo.Dialogs", "UserTwoId", "dbo.Users", "Id");
            AddForeignKey("dbo.Dialogs", "UserOneId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
