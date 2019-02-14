using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictTeam
    {
        public int DictTeamID { get; set; }
        public int DictTeamForeignID { get; set; }
        public int DictDepartmentID { get; set; } //FK
        [Display(Name = "Department")]
        public DictDepartment DictDepartment { get; set; }
        [Required]
        [Display(Name = "Team")]
        public string Name { get; set; }
    }
}