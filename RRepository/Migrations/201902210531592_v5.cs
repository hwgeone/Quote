namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExchangeRates", "YearMonthDate", c => c.String());
            DropColumn("dbo.ExchangeRates", "YearMonth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExchangeRates", "YearMonth", c => c.String());
            DropColumn("dbo.ExchangeRates", "YearMonthDate");
        }
    }
}
