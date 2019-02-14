using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class DictRoles
    {
        public int DictRolesId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}