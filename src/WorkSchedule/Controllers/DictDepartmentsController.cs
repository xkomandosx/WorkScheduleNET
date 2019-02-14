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
using System.Web.Security;

namespace WorkSchedule.Controllers
{
    public class DictDepartmentsController : Controller
    {
        private ScheduleContext db = new ScheduleContext();

        // GET: DictDepartments
        public ActionResult Index()
        {
            return View(db.DictDepartments.ToList());
        }

        // GET: DictDepartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DictDepartment dictDepartment = db.DictDepartments.Find(id);
            if (dictDepartment == null)
            {
                return HttpNotFound();
            }
            return View(dictDepartment);
        }

        // GET: DictDepartments/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">No permission for the requested action.</div>";
                return View("Index", db.DictDepartments.ToList());
            }

            return View();
        }

        // POST: DictDepartments/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DictDepartmentID,DictDepartmentForeignID,Name")] DictDepartment dictDepartment)
        {
            if (ModelState.IsValid)
            {
                db.DictDepartments.Add(dictDepartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictDepartment);
        }

        // GET: DictDepartments/Edit/5
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
            DictDepartment dictDepartment = db.DictDepartments.Find(id);
            if (dictDepartment == null)
            {
                return HttpNotFound();
            }

            if (!Roles.IsUserInRole(User.Identity.Name, "Admin"))
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">No permission for the requested action.</div>";
                return View("Index", db.DictDepartments.ToList());
            }

            return View(dictDepartment);
        }

        // POST: DictDepartments/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DictDepartmentID,DictDepartmentForeignID,Name")] DictDepartment dictDepartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dictDepartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dictDepartment);
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
