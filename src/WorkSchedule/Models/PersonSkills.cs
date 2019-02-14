using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class PersonSkills
    {
        public int PersonSkillsId { get; set; }
        public int DictSkillID { get; set; }
        [DefaultValue(1)]
        public int Level { get; set; }
        public int PersonId { get; set; } //FK
        [Display(Name = "Person")]
        public Person Person { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DefaultValue("1900-01-01")]
        public DateTime DateFrom { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [DefaultValue("2999-01-01")]
        public DateTime DateTo { get; set; }
    }
}