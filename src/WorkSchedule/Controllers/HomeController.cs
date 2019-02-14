using WorkSchedule.DAL;
using WorkSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorkSchedule.Models.Security;

namespace WorkSchedule.Controllers
{
    public class HomeController : Controller
    {
        ScheduleContext db = new ScheduleContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WorkSchedule()
        {

            ViewBag.Message = "Work Schedule";

            return View();
        }


        public ActionResult Config()
        {

            ViewBag.Message = "Configuration";

            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return View("Index");
            }
            return View();
        }


    }
}