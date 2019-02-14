using WorkSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkSchedule.ViewModels
{
    public class ScheduleList
    {
        public List<Schedule> Schedule { get; set; }
        public List<Person> Person { get; set; }

    }
}