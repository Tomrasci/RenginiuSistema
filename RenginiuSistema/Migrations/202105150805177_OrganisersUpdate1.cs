namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganisersUpdate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organisers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "Organiser_Id", c => c.Int());
            AddColumn("dbo.OrgTasks", "Organiser_Id", c => c.Int());
            CreateIndex("dbo.Events", "Organiser_Id");
            CreateIndex("dbo.OrgTasks", "Organiser_Id");
            AddForeignKey("dbo.Events", "Organiser_Id", "dbo.Organisers", "Id");
            AddForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrgTasks", "Organiser_Id", "dbo.Organisers");
            DropForeignKey("dbo.Events", "Organiser_Id", "dbo.Organisers");
            DropIndex("dbo.OrgTasks", new[] { "Organiser_Id" });
            DropIndex("dbo.Events", new[] { "Organiser_Id" });
            DropColumn("dbo.OrgTasks", "Organiser_Id");
            DropColumn("dbo.Events", "Organiser_Id");
            DropTable("dbo.Organisers");
        }
    }
}
