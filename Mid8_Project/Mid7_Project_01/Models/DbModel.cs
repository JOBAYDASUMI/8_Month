using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mid7_Project_01.Models
{
    public enum Grad { G01,G02}
    
    public class Team
    {
        [Key]
        public int TeamId { get; set; }
        [Required,StringLength(50)]
        public string TeamName { get; set; }
        [Required, StringLength(50)]
        public string Coach { get; set; }
        [EnumDataType(typeof(Grad))]
        public Grad Grad { get; set; }
        [Required, StringLength(50)]
        public  string Picture { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
    public class TeamMember
    {
        [Key]
        public int TeamMemberId { get; set; }
        [Required, StringLength(50), Display(Name ="Member name")]
        public string TeamMemberName { get; set; }
        [Required,Column(TypeName = "date")]
        public DateTime BirthDate { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required,ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

    }
    public class TeamDbContext : DbContext
    {
        public TeamDbContext()
        {
            
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
    }
    
}