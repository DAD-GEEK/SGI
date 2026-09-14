namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RolesIdentity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetRoles", "Description", c => c.String(maxLength: 200));
            AddColumn("dbo.AspNetRoles", "BitActivo", c => c.Boolean());
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
            DropColumn("dbo.AspNetRoles", "BitActivo");
            DropColumn("dbo.AspNetRoles", "Description");
        }
    }
}
