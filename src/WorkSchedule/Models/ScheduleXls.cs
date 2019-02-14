using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class ScheduleXls
    {
 
            public int ScheduleId { get; set; }
            [Required]
            public string FullName { get; set; }
            [Required]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
            [Display(Name = "Date start")]
            public DateTime DateStart { get; set; }
            [Required]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
            [Display(Name = "Date end")]
            public DateTime DateEnd { get; set; }
            [Required]
            [Display(Name = "Schedule Status")]
            public string ScheduleStatus { get; set; }
            [Display(Name = "Note")]
            public string ScheduleDesc { get; set; }



    }
}