namespace Mid7_Project_01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamMembers",
                c => new
                    {
                        TeamMemberId = c.Int(nullable: false, identity: true),
                        TeamMemberName = c.String(nullable: false, maxLength: 50),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        Phone = c.String(nullable: false),
                        TeamId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TeamMemberId)
                .ForeignKey("dbo.Teams", t => t.TeamId, cascadeDelete: true)
                .Index(t => t.TeamId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false, maxLength: 50),
                        Coach = c.String(nullable: false, maxLength: 50),
                        Grad = c.Int(nullable: false),
                        Picture = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TeamId);
            CreateStoredProcedure("dbo.DeleteTeam",
                t => new
                {
                    id = t.Int()
                }, "DELETE FROM Teams WHERE TeamId=@id");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamMembers", "TeamId", "dbo.Teams");
            DropIndex("dbo.TeamMembers", new[] { "TeamId" });
            DropTable("dbo.Teams");
            DropTable("dbo.TeamMembers");
            DropStoredProcedure("dbo.DeleteTeam");
        }
    }
}
