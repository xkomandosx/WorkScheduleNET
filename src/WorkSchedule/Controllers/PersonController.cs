using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WorkSchedule.Models.Security;
using WorkSchedule.DAL;
using WorkSchedule.Models;

namespace WorkSchedule.Controllers
{
    public class PersonController : Controller
    {
        private ScheduleContext db = new ScheduleContext();

        // GET: Person
        public ActionResult Index()
        {
            var persons = db.Persons.Include(p => p.DictDepartment).Include(p => p.DictTeam).Include(p => p.DictSkill).Include(p => p.DictJobTitle);
            return View(persons.ToList());
        }

        // GET: Person/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Include(p => p.DictDepartment).Include(p => p.DictTeam).Include(p => p.DictSkill).Include(p => p.DictJobTitle).SingleOrDefault(x => x.PersonId == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name");
            ViewBag.DictJobTitleID = new SelectList(db.DictJobTitles, "DictJobTitleID", "Name");
            ViewBag.DictSkillID = new SelectList(db.DictSkills, "DictSkillID", "Name");
            ViewBag.DictTeamID = new SelectList(db.DictTeams, "DictTeamID", "Name");
            return View();
        }

        // POST: Person/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,Name,Surname,FullName,Login,active,DictDepartmentID,DictTeamID,FormOfEmployment,TypeOfEmployment,DictJobTitleID,DictSkillID")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.active = true;
                person.DateFrom = new DateTime(1999, 1, 1);
                person.DateTo = new DateTime(2999, 1, 1);
                Person exist = db.Persons.Where(x => x.Login == person.Login).FirstOrDefault();
                if (exist != null) // checks the existence of an element in the database
                {
                    ModelState.AddModelError("", "An employee with a given login exists in the database");
                    ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", person.DictDepartmentID);
                    ViewBag.DictJobTitleID = new SelectList(db.DictJobTitles, "DictJobTitleID", "Name", person.DictJobTitleID);
                    ViewBag.DictSkillID = new SelectList(db.DictSkills, "DictSkillID", "Name", person.DictSkillID);
                    ViewBag.DictTeamID = new SelectList(db.DictTeams, "DictTeamID", "Name", person.DictTeamID);
                    return View(person);
                }
                db.Persons.Add(person);
                db.SaveChanges();

                PersonInRoles personIn = new PersonInRoles
                {
                    PersonId = person.PersonId,
                    DictRolesId = 2, //default - user
                    DateFrom = new DateTime(1999, 1, 1),
                    DateTo = new DateTime(2999, 1, 1)
                };
                db.PersonInRoles.Add(personIn);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", person.DictDepartmentID);
            ViewBag.DictJobTitleID = new SelectList(db.DictJobTitles, "DictJobTitleID", "Name", person.DictJobTitleID);
            ViewBag.DictSkillID = new SelectList(db.DictSkills, "DictSkillID", "Name", person.DictSkillID);
            ViewBag.DictTeamID = new SelectList(db.DictTeams, "DictTeamID", "Name", person.DictTeamID);
            return View(person);
        }

        // GET: Person/Edit/5
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
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", person.DictDepartmentID);
            ViewBag.DictJobTitleID = new SelectList(db.DictJobTitles, "DictJobTitleID", "Name", person.DictJobTitleID);
            ViewBag.DictSkillID = new SelectList(db.DictSkills, "DictSkillID", "Name", person.DictSkillID);
            ViewBag.DictTeamID = new SelectList(db.DictTeams, "DictTeamID", "Name", person.DictTeamID);
            return View(person);
        }

        // POST: Person/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,Name,Surname,FullName,Login,active,DictDepartmentID,DictTeamID,FormOfEmployment,TypeOfEmployment,DictJobTitleID,DictSkillID,DateTo")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.DateFrom = new DateTime(1999, 1, 1);
                var test = person;
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();

                var DateTo = new DateTime(1999, 1, 1);
                if (person.DateTo != DateTo)
                {
                    var schedule = db.Schedules.Where(x => x.PersonId == person.PersonId && x.DateStart >= person.DateTo);
                    foreach (var item in schedule)
                    {
                        item.active = false;
                        db.Entry(item).State = EntityState.Modified;  
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.DictDepartmentID = new SelectList(db.DictDepartments, "DictDepartmentID", "Name", person.DictDepartmentID);
            ViewBag.DictJobTitleID = new SelectList(db.DictJobTitles, "DictJobTitleID", "Name", person.DictJobTitleID);
            ViewBag.DictSkillID = new SelectList(db.DictSkills, "DictSkillID", "Name", person.DictSkillID);
            ViewBag.DictTeamID = new SelectList(db.DictTeams, "DictTeamID", "Name", person.DictTeamID);
            return View(person);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetUserName(string UserId)
        {
            
            var searcher = new DirectorySearcher("LDAP://" + UserId.Split('\\').First().ToLower())
            {
                Filter = "(&(ObjectClass=person)(sAMAccountName=" + UserId.Split('\\').Last().ToLower() + "))"
            };

            if (searcher.FindOne() == null)
            {
                return Json(new
                {
                    success = true,
                    fullName = "",
                    name = "",
                    surname = ""
                }, JsonRequestBehavior.AllowGet);
            }
            
            var FullName = searcher.FindOne().Properties["name"][0].ToString();
            var Name = searcher.FindOne().Properties["givenName"][0].ToString();
            var Surname = searcher.FindOne().Properties["sn"][0].ToString();

            return Json(new
            {
                success = true,
                fullName = FullName,
                name = Name,
                surname = Surname
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
