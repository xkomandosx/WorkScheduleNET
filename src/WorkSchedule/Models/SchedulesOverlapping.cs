﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class SchedulesOverlapping
    {
        [Key]
        public int ScheduleId { get; set; }
        [Required]
        public int PersonId { get; set; } //FK
        public Person Person { get; set; }
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
        [ForeignKey("Workplace")]
        public int? WorkplaceId { get; set; } //FK
        public Workplace Workplace { get; set; }
        [Required]
        public bool active { get; set; }
        [Display(Name = "Schedule Status2")]
        public string ScheduleStatus2 { get; set; }
        public int secondsoverlap { get; set; }
    }

}