using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class ReportSaved
    {
        public int ReportSavedId { get; set; }
        [Required]
        [Display(Name = "View Name")]
        public string Name { get; set; }
        public string State { get; set; }
        [Required]
        [Display(Name = "Is global")]
        [DefaultValue("false")]
        public bool global { get; set; }
        public int PersonId { get; set; } //FK
        [Display(Name = "Person")]
        public Person Person { get; set; }
    }
}