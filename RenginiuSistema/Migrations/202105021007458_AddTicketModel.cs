namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTicketModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Single(nullable: false),
                        Code = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Event_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.Event_Id)
                .Index(t => t.Event_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Event_Id", "dbo.Events");
            DropIndex("dbo.Tickets", new[] { "Event_Id" });
            DropTable("dbo.Tickets");
        }
    }
}
