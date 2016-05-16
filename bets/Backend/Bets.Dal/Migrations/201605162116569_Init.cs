namespace Bets.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Game = c.String(),
                        Tournament = c.String(),
                        Forecast = c.String(),
                        Content = c.String(),
                        Coefficient = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GameStartDate = c.DateTime(nullable: false),
                        ShowDate = c.DateTime(nullable: false),
                        MakeDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Bets");
        }
    }
}
