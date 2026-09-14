namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AspNetRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "BitDefault", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "BitDefault");
        }
    }
}
