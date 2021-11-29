namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSomething1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Invoices", "InvoiceNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invoices", "InvoiceNumber", c => c.String());
        }
    }
}
