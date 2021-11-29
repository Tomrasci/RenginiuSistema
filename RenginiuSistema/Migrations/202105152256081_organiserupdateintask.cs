namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class organiserupdateintask : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers");
            DropIndex("dbo.OrgTasks", new[] { "Organiser_Id" });
            AlterColumn("dbo.OrgTasks", "Organiser_Id", c => c.Int(nullable: true));
            CreateIndex("dbo.OrgTasks", "Organiser_Id");
            AddForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers");
            DropIndex("dbo.OrgTasks", new[] { "Organiser_Id" });
            AlterColumn("dbo.OrgTasks", "Organiser_Id", c => c.Int());
            CreateIndex("dbo.OrgTasks", "Organiser_Id");
            AddForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers", "Id");
        }
    }
}
