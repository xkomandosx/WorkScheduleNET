using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WorkSchedule.Models.Security
{
    public class Audit
    {
        public int AuditId { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime DateTimeStamp { get; set; }
        public string ChangedBy { get; set; }
    }
}