namespace Mooc.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intiDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Student",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfessorId = c.Long(nullable: false),
                        TeacherName = c.String(),
                        Level = c.String(),
                        Department = c.String(),
                        Company = c.String(),
                        Introduction = c.String(),
                        AddTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 100),
                        PassWord = c.String(nullable: false, maxLength: 100),
                        Email = c.String(maxLength: 100),
                        UserState = c.Int(nullable: false),
                        RoleType = c.Int(nullable: false),
                        AddTime = c.DateTime(),
                        Gender = c.String(),
                        StudentNo = c.String(),
                        Faulty = c.String(),
                        Major = c.String(),
                        CountryId = c.Int(nullable: false),
                        ProfessorGuid = c.String(),
                        ProfessorId = c.Long(nullable: false),
                        PhotoFileName = c.String(),
                        NickName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.Teachers");
            DropTable("dbo.Student");
        }
    }
}
