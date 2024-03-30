namespace Mid7_Project_01.Migrations
{
    using Mid7_Project_01.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mid7_Project_01.Models.TeamDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Mid7_Project_01.Models.TeamDbContext";
        }

        protected override void Seed(Mid7_Project_01.Models.TeamDbContext context)
        {
            Team t = new Team { TeamName = "A1", Coach = "C1", Grad = Grad.G01, IsActive = true, Picture = "1.jpg" };
            t.TeamMembers.Add(new TeamMember { TeamMemberName = "M1", BirthDate = new DateTime(2010, 11, 3), Phone = "01710XXXXXX" });
            context.Teams.Add(t);
            context.SaveChanges();
        }
    }
}
