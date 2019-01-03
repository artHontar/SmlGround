namespace SmlGround.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SocialDb1209 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Messages");
            DropColumn("dbo.Messages", "MyId");
            AddColumn("dbo.Messages", "MessageId", c => c.Long(nullable: false, identity: true));
            AddColumn("dbo.Messages", "SenderId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Messages", "ReceiverId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Messages", "MessageId");
            CreateIndex("dbo.Messages", "SenderId");
            CreateIndex("dbo.Messages", "ReceiverId");
            AddForeignKey("dbo.Messages", "ReceiverId", "dbo.Users", "Id");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "MyId", c => c.Long(nullable: false, identity: true));
            DropForeignKey("dbo.Messages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropPrimaryKey("dbo.Messages");
            DropColumn("dbo.Messages", "ReceiverId");
            DropColumn("dbo.Messages", "SenderId");
            DropColumn("dbo.Messages", "MessageId");
            AddPrimaryKey("dbo.Messages", "MyId");
        }
    }
}
