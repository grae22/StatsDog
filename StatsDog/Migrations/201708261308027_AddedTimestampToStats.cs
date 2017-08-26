namespace StatsDog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimestampToStats : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stats", "Timestamp", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stats", "Timestamp");
        }
    }
}
