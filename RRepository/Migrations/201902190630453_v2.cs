namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.QuoteRevenues", "TotalBooking", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.QuoteRevenues", "TotalBooking", c => c.String());
        }
    }
}
