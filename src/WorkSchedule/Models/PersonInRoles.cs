using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class PersonInRoles
    {
        public int PersonInRolesId { get; set; }
        [Required]
        public int PersonId { get; set; } //FK
        public Person Person { get; set; }
        [Required]
        public int DictRolesId { get; set; } //FK
        public DictRoles DictRoles { get; set; }
        [Required]
        [DefaultValue("1900-01-01")]
        public DateTime DateFrom { get; set; }
        [Required]
        [DefaultValue("2999-01-01")]
        public DateTime DateTo { get; set; }
    }
}