using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class Workplace
    {
        public int WorkplaceId { get; set; }
        [Required]
        public string Name { get; set; }
        public int DictDepartmentID { get; set; } //FK
        public DictDepartment DictDepartment { get; set; }
    }
}