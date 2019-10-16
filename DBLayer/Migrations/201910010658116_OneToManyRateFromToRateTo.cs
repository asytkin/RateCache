namespace DBLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneToManyRateFromToRateTo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RateFroms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(nullable: false, maxLength: 3),
                        CreatingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RateToes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        To = c.String(nullable: false, maxLength: 3),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RateFrom_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RateFroms", t => t.RateFrom_Id, cascadeDelete: true)
                .Index(t => t.RateFrom_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RateToes", "RateFrom_Id", "dbo.RateFroms");
            DropIndex("dbo.RateToes", new[] { "RateFrom_Id" });
            DropTable("dbo.RateToes");
            DropTable("dbo.RateFroms");
        }
    }
}
