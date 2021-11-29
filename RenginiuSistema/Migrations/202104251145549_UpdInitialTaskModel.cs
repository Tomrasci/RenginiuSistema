namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdInitialTaskModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrgTasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        State = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateCompleted = c.DateTime(nullable: true),
                        Deadline = c.DateTime(nullable: true),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgTasks", "Event_Id", "dbo.Events");
            DropIndex("dbo.OrgTasks", new[] { "Event_Id" });
            DropTable("dbo.OrgTasks");
        }
    }
}
