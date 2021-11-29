namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateInitialEventsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Setting", c => c.String());
            AlterColumn("dbo.Events", "Name", c => c.String(nullable: false));
            DropColumn("dbo.Events", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Location", c => c.String());
            AlterColumn("dbo.Events", "Name", c => c.String());
            DropColumn("dbo.Events", "Setting");
        }
    }
}
