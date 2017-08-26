namespace StatsDog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStats : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationName = c.String(maxLength: 64),
                        ApplicationVersion = c.String(maxLength: 16),
                        SourceName = c.String(maxLength: 128),
                        EventName = c.String(maxLength: 32),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stats");
        }
    }
}
