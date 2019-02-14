using WorkSchedule.DAL;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WorkSchedule
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ScheduleContext db = new ScheduleContext();
        public string GetUserName()
        {
            string UserId = User.Identity.Name;
            var searcher = new DirectorySearcher("LDAP://" + UserId.Split('\\').First().ToLower())
            {
                Filter = "(&(ObjectClass=person)(sAMAccountName=" + UserId.Split('\\').Last().ToLower() + "))"
            };

            var result = searcher.FindOne();
            if (result == null)
                return string.Empty;

            return result.Properties["name"][0].ToString();
        }
        public string GetUserDepartament()
        {
            string UserId = User.Identity.Name;
            var dateNow = DateTime.Now.Date;
            var userDep = from p in db.Persons
                          join d in db.DictDepartments
                          on p.DictDepartmentID equals d.DictDepartmentID
                          where p.Login == UserId && p.DateFrom <= dateNow && p.DateTo >= dateNow && p.active == true
                          select new { d.Name };
            var depName = userDep.ToArray();
            if (depName.Length == 0)
                return "";
            else    
                return depName[0].Name;

        }
        public string GetUserTeam()
        {
            string UserId = User.Identity.Name;
            var dateNow = DateTime.Now.Date;
            var userTeam = from p in db.Persons
                          join d in db.DictTeams
                          on p.DictTeamID equals d.DictTeamID
                          where p.Login == UserId && p.DateFrom <= dateNow && p.DateTo >= dateNow && p.active == true
                          select new { d.Name };
            var teamName = userTeam.ToArray();

            if (teamName.Length == 0)
                return "";
            else
                return teamName[0].Name;
        }
        public string GetUserRoles()
        {
            string UserId = User.Identity.Name;
            var dateNow = DateTime.Now.Date;
            var userRoles = from p in db.Persons
                            join pr in db.PersonInRoles
                            on p.PersonId equals pr.PersonId
                            join r in db.DictRoles
                            on pr.DictRolesId equals r.DictRolesId
                            where p.Login == UserId && p.DateFrom <= dateNow && p.DateTo >= dateNow && p.active == true
                            select new { r.Name };

            var roleName = userRoles.Select(x => x.Name).ToArray();

            return string.Join("",roleName);
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {

            if (Session["UserFullName"] == null)
                Session["UserFullName"] = GetUserName();

            if (Session["UserDepName"] == null)
                Session["UserDepName"] = GetUserDepartament();

            //if (Session["UserTeamName"] == null)
            //    Session["UserTeamName"] = GetUserTeam();

            if (Session["UserRoleName"] == null)
                Session["UserRoleName"] = GetUserRoles();
        }
    }
}
