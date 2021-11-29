namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TaskUpd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrgTasks", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrgTasks", "Name", c => c.String());
        }
    }
}
