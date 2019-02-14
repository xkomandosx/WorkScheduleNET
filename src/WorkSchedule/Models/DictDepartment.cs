using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictDepartment
    {
        public int DictDepartmentID { get; set; }
        public int DictDepartmentForeignID { get; set; }
        [Required]
        [Display(Name = "Department")]
        public string Name { get; set; }
    }
}