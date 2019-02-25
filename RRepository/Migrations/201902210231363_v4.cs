namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "ExchageRate", c => c.Single(nullable: false));
            AddColumn("dbo.Incomes", "Comment", c => c.String());
            AlterColumn("dbo.ExchangeRates", "Rate", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExchangeRates", "Rate", c => c.String());
            DropColumn("dbo.Incomes", "Comment");
            DropColumn("dbo.Incomes", "ExchageRate");
        }
    }
}
