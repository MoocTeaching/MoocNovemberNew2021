namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01032021 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teachers", "MongodbImgId", c => c.String());
            DropColumn("dbo.Teachers", "ProfessorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teachers", "ProfessorId", c => c.Long(nullable: false));
            DropColumn("dbo.Teachers", "MongodbImgId");
        }
    }
}
