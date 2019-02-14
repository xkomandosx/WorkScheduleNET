using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictJobTitle
    {
        public int DictJobTitleID { get; set; }
        [Required]
        [Display(Name = "Job title")]
        public string Name { get; set; }
    }
}