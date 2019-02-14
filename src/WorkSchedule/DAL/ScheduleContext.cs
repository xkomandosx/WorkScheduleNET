using WorkSchedule.Models;
using WorkSchedule.Models.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkSchedule.DAL
{
    public class ScheduleContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ScheduleContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        public ScheduleContext() : base("name=WorkSchedule")
        {
        }
        
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Workplace> Workplaces { get; set; }
        public virtual DbSet<ScheduleExtraAdds> ScheduleExtraAdds { get; set; }
        public virtual DbSet<DictDepartment> DictDepartments { get; set; }
        public virtual DbSet<DictScheduleStatus> DictScheduleStatus { get; set; }
        public virtual DbSet<DictDaysOff> DictDaysOffs { get; set; }
        public virtual DbSet<DictRoles> DictRoles { get; set; }
        public virtual DbSet<DictTeam> DictTeams { get; set; }
        public virtual DbSet<PersonInRoles> PersonInRoles { get; set; }
        public virtual DbSet<Audit> Audit { get; set; }
        public virtual DbSet<SchedulesOverlapping> SchedulesOverlapping { get; set; }
        public virtual DbSet<DictSkill> DictSkills { get; set; }
        public virtual DbSet<DictJobTitle> DictJobTitles { get; set; }
        public virtual DbSet<PersonSkills> PersonSkills { get; set; }
        public virtual DbSet<ReportSaved> ReportSaved { get; set; }

        object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry = ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => p.State == EntityState.Modified || p.State == EntityState.Added).ToList();
            var now = DateTime.Now;

            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;
                if (change.State.ToString() == "Added")
                {
                    base.SaveChanges();
                    var primaryKey = GetPrimaryKeyValue(change);
                    foreach (var prop in change.CurrentValues.PropertyNames)
                    {
                        var currentValue = (change.CurrentValues[prop] == null) ? "" : change.CurrentValues[prop].ToString();

                        Audit log = new Audit()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = "",
                            NewValue = currentValue,
                            DateTimeStamp = now,
                            ChangedBy = HttpContext.Current.User.Identity.Name
                        };
                        Audit.Add(log);
                    }
                }
                else
                {
                    var primaryKey = GetPrimaryKeyValue(change);

                    foreach (var prop in change.OriginalValues.PropertyNames)
                    {
                        var originalValue = (change.GetDatabaseValues().GetValue<object>(prop) == null) ? "" : change.GetDatabaseValues().GetValue<object>(prop).ToString();
                        var currentValue = (change.CurrentValues[prop] == null) ? "" : change.CurrentValues[prop].ToString();

                        if (originalValue != currentValue)
                        {
                            Audit log = new Audit()
                            {
                                EntityName = entityName,
                                PrimaryKeyValue = primaryKey.ToString(),
                                PropertyName = prop,
                                OldValue = originalValue,
                                NewValue = currentValue,
                                DateTimeStamp = now,
                                ChangedBy = HttpContext.Current.User.Identity.Name
                            };
                            Audit.Add(log);
                        }
                    }
                }
                
                
            }
            return base.SaveChanges();
        }
    }
}