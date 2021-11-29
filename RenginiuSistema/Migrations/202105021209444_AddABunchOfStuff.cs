namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddABunchOfStuff : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceNumber = c.String(),
                        Price = c.Single(nullable: false),
                        State = c.Int(nullable: false),
                        DatePaid = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.Int(nullable: false),
                        DateCreated = c.DateTime(),
                        State = c.Int(nullable: false),
                        Client_Id = c.String(maxLength: 128),
                        Invoice_Id = c.Int(),
                        Ticket_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Client_Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .ForeignKey("dbo.Tickets", t => t.Ticket_Id, cascadeDelete: true)
                .Index(t => t.Client_Id)
                .Index(t => t.Invoice_Id)
                .Index(t => t.Ticket_Id);
            
            AddColumn("dbo.Events", "TicketPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Ticket_Id", "dbo.Tickets");
            DropForeignKey("dbo.Orders", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Orders", "Client_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "Ticket_Id" });
            DropIndex("dbo.Orders", new[] { "Invoice_Id" });
            DropIndex("dbo.Orders", new[] { "Client_Id" });
            DropColumn("dbo.Events", "TicketPrice");
            DropTable("dbo.Orders");
            DropTable("dbo.Invoices");
        }
    }
}
