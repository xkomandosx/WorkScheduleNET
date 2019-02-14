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

namespace WorkSchedule.Controllers
{
    public class ReportController : Controller
    {
        private ScheduleContext db = new ScheduleContext();
        // GET: Report
        public ActionResult Index()
        {
            var reportSaved = db.ReportSaved.Include(r => r.Person);
            ViewBag.ReportSavedId = new SelectList(db.ReportSaved.Include(r => r.Person).Where(x => x.Person.Login == User.Identity.Name || x.global == true).OrderByDescending(p => p.global).ThenBy(p => p.Name), "State", "Name");
            return View();
        }


        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Report/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Report/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name, State")] ReportSaved reportSaved, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                reportSaved.global = false;
                reportSaved.PersonId = db.Persons.Where(x => x.Login == User.Identity.Name).FirstOrDefault().PersonId;
                reportSaved.State = collection["State"];
                db.ReportSaved.Add(reportSaved);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.ReportSavedId = new SelectList(db.ReportSaved, "State", "Name");
            return View("Index", reportSaved);
        }

        // GET: Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
