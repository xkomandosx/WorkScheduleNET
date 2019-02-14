namespace WorkSchedule.Migrations
{
    using WorkSchedule.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WorkSchedule.DAL.ScheduleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WorkSchedule.DAL.ScheduleContext context)
        {

            context.DictDepartments.AddOrUpdate(x => x.Name,
                new DictDepartment() { DictDepartmentForeignID = 599, Name = "Department FO" },
                new DictDepartment() { DictDepartmentForeignID = 598, Name = "Department BO" }
                );
            context.DictRoles.AddOrUpdate(x => x.Name,
                new DictRoles() { Name = "Admin" },
                new DictRoles() { Name = "User" },
                new DictRoles() { Name = "Moderator" }
                );
        }
    }
}
