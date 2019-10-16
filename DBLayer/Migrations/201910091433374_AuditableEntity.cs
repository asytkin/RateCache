namespace DBLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditableEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RateFroms", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RateFroms", "CreatedBy", c => c.String(maxLength: 256));
            AddColumn("dbo.RateFroms", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.RateFroms", "UpdatedBy", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RateFroms", "UpdatedBy");
            DropColumn("dbo.RateFroms", "UpdatedDate");
            DropColumn("dbo.RateFroms", "CreatedBy");
            DropColumn("dbo.RateFroms", "CreatedDate");
        }
    }
}
