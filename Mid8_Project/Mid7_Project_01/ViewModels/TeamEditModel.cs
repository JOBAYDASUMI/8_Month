using Mid7_Project_01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mid7_Project_01.ViewModels
{
    public class TeamEditModel
    {
        public int TeamId { get; set; }
        [Required, StringLength(50)]
        public string TeamName { get; set; }
        [Required, StringLength(50)]
        public string Coach { get; set; }
        [EnumDataType(typeof(Grad))]
        public Grad Grad { get; set; }
        
        public HttpPostedFileBase Picture { get; set; }
        public bool IsActive { get; set; }
        public virtual List<TeamMember> TeamMembers { get; set; } = new List<TeamMember>();
    }
}