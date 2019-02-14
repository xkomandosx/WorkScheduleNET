using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models
{
    public class ScheduleExtraAdds
    {
        public int ScheduleExtraAddsId { get; set; }
        public int PersonId { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string ScheduleExtraAddsStatus { get; set; }
        public string ScheduleExtraAddsDesc { get; set; }
        public bool active { get; set; }
    }
}