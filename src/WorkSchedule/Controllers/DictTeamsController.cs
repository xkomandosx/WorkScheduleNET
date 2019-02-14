using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorkSchedule.DAL;
using WorkSchedule.Models;

namespace WorkSchedule.Controllers
{
    public class DictTeamsController : Controller
    {
        private ScheduleContext db = new ScheduleContext();

        // GET: DictTeams
        public ActionResult Index()
        {
            var dictTeams = db.DictTeams.Include(d => d.DictDepartment);
            return View(dictTeams.ToList());
        }

        // GET: DictTeams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictTeam dictTeam = db.DictTeams.Find(id);
            if (dictTeam == null)
            {
                return HttpNotFound();
            }
            return View(dictTeam);
        }

        // GET: DictTeams/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name");
            return View();
        }

        // POST: DictTeams/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictTeamID,DictTeamForeignID,DictDepartmentID,Name")] DictTeam dictTeam)
        {
            if (ModelState.IsValid)
            {
                db.DictTeams.Add(dictTeam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictTeam.DictDepartmentID);
            return View(dictTeam);
        }

        // GET: DictTeams/Edit/5
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
            DictTeam dictTeam = db.DictTeams.Find(id);
            if (dictTeam == null)
            {
                return HttpNotFound();
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictTeam.DictDepartmentID);
            return View(dictTeam);
        }

        // POST: DictTeams/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictTeamID,DictTeamForeignID,DictDepartmentID,Name")] DictTeam dictTeam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictTeam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", dictTeam.DictDepartmentID);
            return View(dictTeam);
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
