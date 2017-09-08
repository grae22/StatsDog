namespace StatsDog.Migrations
{
  using System.Data.Entity.Migrations;

  public partial class AddedDataFieldToStats : DbMigration
  {
    public override void Up()
    {
      AddColumn("dbo.Stats", "Data", c => c.String(maxLength: 64));

      Sql("UPDATE dbo.Stats SET Data=''");
    }

    public override void Down()
    {
      DropColumn("dbo.Stats", "Data");
    }
  }
}
