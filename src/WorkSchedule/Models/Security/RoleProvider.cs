using WorkSchedule.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace WorkSchedule.Models.Security
{
    public class WorkScheduleRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            { 
                using (ScheduleContext db = new ScheduleContext())
                {
                    Person user = db.Persons.Where(u => u.Login == username).FirstOrDefault();
                    
                    var roles = from ur in db.Persons
                                from ur_r in db.PersonInRoles
                                from r in db.DictRoles
                                where ur.PersonId == ur_r.PersonId && ur_r.DictRolesId == r.DictRolesId && ur.PersonId == user.PersonId
                                select r.Name;
                    if (roles != null)
                        return roles.ToArray();
                    else
                        return new string[] { }; ;
                }
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            {
                using (ScheduleContext db = new ScheduleContext())
                {
                    Person user = db.Persons.FirstOrDefault(u => u.Login.Equals(username, StringComparison.CurrentCultureIgnoreCase) || u.FullName.Equals(username, StringComparison.CurrentCultureIgnoreCase));

                    var roles = from ur in db.Persons
                                from ur_r in db.PersonInRoles
                                from r in db.DictRoles
                                where ur.PersonId == ur_r.PersonId && ur_r.DictRolesId == r.DictRolesId && ur.PersonId == user.PersonId
                                select r.Name;
                    if (user != null)
                        return roles.Any(r => r.Equals(roleName, StringComparison.CurrentCultureIgnoreCase));
                    else
                        return false;
                }
            }
        }
        
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}