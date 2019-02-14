using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictScheduleStatus
    {
        public int DictScheduleStatusID { get; set; }
        [Required]
        [Display(Name = "Schedule status")]
        public string Name { get; set; }
        public string ColorHTML { get; set; }
    }
}