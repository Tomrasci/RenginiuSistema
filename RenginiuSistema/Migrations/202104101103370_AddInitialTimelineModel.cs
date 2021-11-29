namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialTimelineModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Timelines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Events", "Timeline_Id", c => c.Int());
            CreateIndex("dbo.Events", "Timeline_Id");
            AddForeignKey("dbo.Events", "Timeline_Id", "dbo.Timelines", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Timeline_Id", "dbo.Timelines");
            DropIndex("dbo.Events", new[] { "Timeline_Id" });
            DropColumn("dbo.Events", "Timeline_Id");
            DropTable("dbo.Timelines");
        }
    }
}
