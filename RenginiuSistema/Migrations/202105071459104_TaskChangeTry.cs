namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskChangeTry : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrgTasks", "Event_Id", "dbo.Events");
            DropIndex("dbo.OrgTasks", new[] { "Event_Id" });
            AlterColumn("dbo.OrgTasks", "Event_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.OrgTasks", "Event_Id");
            AddForeignKey("dbo.OrgTasks", "Event_Id", "dbo.Events", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgTasks", "Event_Id", "dbo.Events");
            DropIndex("dbo.OrgTasks", new[] { "Event_Id" });
            AlterColumn("dbo.OrgTasks", "Event_Id", c => c.Int());
            CreateIndex("dbo.OrgTasks", "Event_Id");
            AddForeignKey("dbo.OrgTasks", "Event_Id", "dbo.Events", "Id");
        }
    }
}
