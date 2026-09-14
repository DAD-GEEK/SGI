namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FechaCumpleaños : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "FechaNacimiento", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FechaNacimiento");
        }
    }
}
