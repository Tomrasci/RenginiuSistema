namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomething : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Ticket_Id", "dbo.Tickets");
            DropIndex("dbo.Orders", new[] { "Ticket_Id" });
            AlterColumn("dbo.Orders", "Ticket_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Ticket_Id");
            AddForeignKey("dbo.Orders", "Ticket_Id", "dbo.Tickets", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Ticket_Id", "dbo.Tickets");
            DropIndex("dbo.Orders", new[] { "Ticket_Id" });
            AlterColumn("dbo.Orders", "Ticket_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "Ticket_Id");
            AddForeignKey("dbo.Orders", "Ticket_Id", "dbo.Tickets", "Id", cascadeDelete: true);
        }
    }
}
