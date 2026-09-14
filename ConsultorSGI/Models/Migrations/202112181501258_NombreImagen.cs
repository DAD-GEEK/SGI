namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NombreImagen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NombreImagen", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NombreImagen");
        }
    }
}
