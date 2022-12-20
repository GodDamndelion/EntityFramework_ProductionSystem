namespace lab_3_EntityFramework_ProductionSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UseInRework : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Use_in");
            AddColumn("dbo.Use_in", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Use_in", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Use_in");
            DropColumn("dbo.Use_in", "Id");
            AddPrimaryKey("dbo.Use_in", new[] { "Type_of_product_Id", "Recipe_Id" });
        }
    }
}
