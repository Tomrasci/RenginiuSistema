namespace RenginiuSistema.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"         
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'df696c9b-fa26-4f46-b2b1-ad753ea50361', N'Organizer')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'df696c9b-fa26-4f46-b2b1-ad753ea50362', N'Manager')
                INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'df696c9b-fa26-4f46-b2b1-ad753ea50363', N'Participant')
                ");
        }
        
        public override void Down()
        {
        }
    }
}
