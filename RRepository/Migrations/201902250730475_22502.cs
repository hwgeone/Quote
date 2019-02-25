namespace RRepository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _22502 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "QuoteNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incomes", "QuoteNumber");
        }
    }
}
