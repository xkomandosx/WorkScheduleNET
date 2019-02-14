using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictDaysOff
    {
        public int DictDaysOffId { get; set; }
        public int DictDepartmentID { get; set; }//FK
        [Display(Name = "Department")]
        public DictDepartment DictDepartment { get; set; }
        [Required]
        public DateTime DateDayOff { get; set; }
    }
}