using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class Person
    {
        public int PersonId { get; set; }
        [Required]
        [Display(Name ="Name")]
        public string Name { get; set; }
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        [Display(Name = "Is active")]
        [DefaultValue("true")]
        public bool active { get; set; }
        public int DictDepartmentID { get; set; } //FK
        [Display(Name = "Department")]
        public DictDepartment DictDepartment { get; set; }
        public int? DictTeamID { get; set; } //FK
        [Display(Name = "Team")]
        public DictTeam DictTeam { get; set; }
        [Display(Name = "Form of employment")]
        public string FormOfEmployment { get; set; }
        [Display(Name = "Type of employment")]
        public decimal TypeOfEmployment { get; set; }
        public int DictJobTitleID { get; set; } //FK
        [Display(Name = "Job title")]
        public DictJobTitle DictJobTitle { get; set; }
        public int DictSkillID { get; set; } //FK
        [Display(Name = "Skill")]
        public DictSkill DictSkill { get; set; }

        [Required]
        [DefaultValue("1900-01-01")]
        public DateTime DateFrom { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DefaultValue("2999-01-01")]
        public DateTime DateTo { get; set; }
    }
}