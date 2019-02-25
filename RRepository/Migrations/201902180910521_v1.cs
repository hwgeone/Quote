namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExchangeRates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Currency = c.String(),
                        Rate = c.String(),
                        YearMonth = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuoteRevenues",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountName = c.String(),
                        OpportunityOwner = c.String(),
                        Territory = c.String(),
                        Type = c.String(),
                        QuoteNumber = c.String(),
                        StudySite = c.String(),
                        ProjectLine = c.String(),
                        AmountCurrency = c.String(),
                        Amount = c.Single(nullable: false),
                        BookingCloseDate = c.String(),
                        TotalBooking = c.String(),
                        KickOffDate = c.String(),
                        Status = c.String(),
                        ProjectClosedDate = c.String(),
                        OracleCurrency = c.String(),
                        CostAmount = c.Single(nullable: false),
                        USDCurrency = c.String(),
                        FinalCost = c.Single(nullable: false),
                        different = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QuoteRevenues");
            DropTable("dbo.ExchangeRates");
        }
    }
}
