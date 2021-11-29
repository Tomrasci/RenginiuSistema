namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdInitialTaskModel1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrgTasks", "DateCompleted", c => c.DateTime());
            AlterColumn("dbo.OrgTasks", "Deadline", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrgTasks", "Deadline", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrgTasks", "DateCompleted", c => c.DateTime(nullable: false));
        }
    }
}
