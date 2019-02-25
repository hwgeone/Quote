namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        OracleCurrency = c.String(),
                        CostAmount = c.Single(nullable: false),
                        StartDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.String(),
                        AmountCurrency = c.String(),
                        Amount = c.Single(nullable: false),
                        BookingCloseDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountName = c.String(),
                        OpportunityOwner = c.String(),
                        Territory = c.String(),
                        QuoteNumber = c.String(),
                        StudySite = c.String(),
                        ProjectLine = c.String(),
                        TotalBooking = c.Single(nullable: false),
                        KickOffDate = c.String(),
                        Status = c.String(),
                        ProjectClosedDate = c.String(),
                        USDCurrency = c.String(),
                        FinalCost = c.Single(nullable: false),
                        different = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.QuoteRevenues");
        }
        
        public override void Down()
        {
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
                        TotalBooking = c.Single(nullable: false),
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
            
            DropTable("dbo.Projects");
            DropTable("dbo.Orders");
            DropTable("dbo.Incomes");
        }
    }
}
