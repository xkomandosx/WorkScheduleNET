using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictSkill
    {
        public int DictSkillID { get; set; }
        [Required]
        [Display(Name = "Skill")]
        public string Name { get; set; }
    }
}