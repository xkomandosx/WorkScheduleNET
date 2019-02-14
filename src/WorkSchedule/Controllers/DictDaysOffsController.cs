using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkSchedule.DAL;
using WorkSchedule.Models;
using System.Data.Entity.Infrastructure;
using System.Web.Security;

namespace WorkSchedule.Controllers
{

    public class DictDaysOffsController : Controller
    {
        private ScheduleContext db = new ScheduleContext();
        
        // GET: DictDaysOffs
        public ActionResult Index()
        {
            //String[] roles = Roles.GetRolesForUser();
            return View(db.DictDaysOffs.Include(d => d.DictDepartment).ToList());
        }

        // GET: DictDaysOffs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictDaysOff dictDaysOff = db.DictDaysOffs.Find(id);
            if (dictDaysOff == null)
            {
                return HttpNotFound();
            }
            return View(dictDaysOff);
        }

        // GET: DictDaysOffs/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">No permission for the requested action.</div>";
                return View("Index",db.DictDaysOffs.Include(d => d.DictDepartment).ToList());
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name");
            return View();
        }

        // POST: DictDaysOffs/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictDaysOffId,DictDepartmentID,DateDayOff")] DictDaysOff dictDaysOff)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DictDaysOffs.Add(dictDaysOff);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictDaysOff.DictDepartmentID);
            return View(dictDaysOff);
        }

        // GET: DictDaysOffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictDaysOff dictDaysOff = db.DictDaysOffs.Find(id);
            if (dictDaysOff == null)
            {
                return HttpNotFound();
            }

            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">No permission for the requested action.</div>";
                return View("Index", db.DictDaysOffs.Include(d => d.DictDepartment).ToList());
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictDaysOff.DictDepartmentID);
            return View(dictDaysOff);
        }

        // POST: DictDaysOffs/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictDaysOffId,DictDepartmentID,DateDayOff")] DictDaysOff dictDaysOff)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(dictDaysOff).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.)
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictDaysOff.DictDepartmentID);
            return View(dictDaysOff);
        }
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
