namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvoiceModels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tickets", "DateCreated", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tickets", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
