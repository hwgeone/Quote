namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v31 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "ProjectId", c => c.Guid(nullable: false));
            AddColumn("dbo.Orders", "ProjectId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ProjectId");
            DropColumn("dbo.Incomes", "ProjectId");
        }
    }
}
