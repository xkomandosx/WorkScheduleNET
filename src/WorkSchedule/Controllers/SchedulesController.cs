using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkSchedule.DAL;
using WorkSchedule.Models;
using LinqToExcel;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using WorkSchedule.Models.Security;
using System.Web.Security;
using Ical.Net;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using System.Text;
using System.Data.Entity.SqlServer;
using WorkSchedule.ViewModels;

namespace WorkSchedule.Controllers
{
    public class SchedulesController : Controller
    {
        public static DateTime RoundDate(DateTime d)
        {
            DateTime dtRounded = new DateTime();
            dtRounded = new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
            if (d.Millisecond >= 500) dtRounded = dtRounded.AddSeconds(1);

            return dtRounded;
        }

        private ScheduleContext db = new ScheduleContext();

        // GET: Schedules
        public ActionResult Index()
        {
            var depName = Session["UserDepName"].ToString();
            var schedules = db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Include(s => s.Person.DictTeam)
                .Where(s => s.active == true && s.Person.DictDepartment.Name == depName);
            return View(schedules.ToList());
        }
        
        // GET: Schedules/Personal
        public ActionResult Personal()
        {
            var depName = Session["UserDepName"].ToString();
            var fullName = Session["UserFullName"].ToString();
            var schedules = db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Include(s => s.Person.DictTeam)
                .Where(s => s.active == true && s.Person.DictDepartment.Name == depName && s.Person.FullName == fullName);
            return View(schedules.ToList());
        }

        public ActionResult Table(FormCollection collection)
        {
            var formOfEmployment = collection["formOfEmployment"];
            var depName = Session["UserDepName"].ToString();
            var fullName = Session["UserFullName"].ToString();
            var schedules = db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Include(s => s.Person.DictTeam)
                .Where(s => s.active == true && s.Person.DictDepartment.Name == depName && s.Person.FullName == fullName);

            ScheduleList viewModel = new ScheduleList();
            if (formOfEmployment == null || formOfEmployment == "all")
            {
                viewModel.Person = db.Persons.Include(p => p.DictTeam).Where(s => s.active == true && s.DictDepartment.Name == depName).OrderBy(p => p.DictTeam.Name).ThenBy(p => p.FullName).ToList();
            }
            else
            {
                viewModel.Person = db.Persons.Include(p => p.DictTeam).Where(s => s.active == true && s.DictDepartment.Name == depName && s.FormOfEmployment == formOfEmployment).OrderBy(p => p.DictTeam.Name).ThenBy(p => p.FullName).ToList();
            }
            viewModel.Schedule = schedules.ToList();
            
            return View(viewModel);
        }

        // GET: Schedules/Team
        public ActionResult Team()
        {
            var depName = Session["UserDepName"].ToString();
            var schedules = db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Include(s => s.Person.DictTeam)
                .Where(s => s.active == true && s.Person.DictDepartment.Name == depName);
            return View(schedules.ToList());
        }


        public ActionResult ExcelImport()
        {
            return View();
        }

        //  Schedules/GetDiaryEvents
        public JsonResult GetDiaryEvents()
        {
            var depName = Session["UserDepName"].ToString();
            var schedules = from s in db.SchedulesOverlapping.Include(s => s.Person).Include(s => s.Workplace).Where(s => s.active == true && s.Person.DictDepartment.Name == depName)
                            join d in db.DictScheduleStatus.DefaultIfEmpty()
                            on s.ScheduleStatus2 equals d.Name
                            into temp
                            from d in temp.DefaultIfEmpty()
                            select new
                            {
                               s.ScheduleId, s.Person, s.DateStart, s.DateEnd, s.ScheduleStatus2, s.ScheduleDesc, s.secondsoverlap,
                               colorHTML = d.ColorHTML == null ? "#FFFF92" : d.ColorHTML
                            }
                            ; 
            var eventList = from e in schedules
                            select new
                            {
                                id = e.ScheduleId,
                                title = e.Person.FullName,
                                start = e.DateStart,
                                end = e.DateEnd,
                                description = e.ScheduleStatus2,
                                note = e.ScheduleDesc,
                                allDay = (SqlFunctions.DateDiff("hour", e.DateStart, e.DateEnd) > 12) ? true : false,
                                color = e.colorHTML,
                                hours = (SqlFunctions.DateDiff("hour", e.DateStart, e.DateEnd) > 12) ? SqlFunctions.DateDiff("day", e.DateStart, e.DateEnd) * 8 : (SqlFunctions.DateDiff("s", e.DateStart, e.DateEnd) - e.secondsoverlap) / (decimal)3600
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        //  Schedules/GetDiaryEventsTimeKey
        public JsonResult GetDiaryEventsTimeKey(string timeKey_select)
        {
            var depName = Session["UserDepName"].ToString();
            var month = Int32.Parse(timeKey_select.Substring(5, 2));
            var year = Int32.Parse(timeKey_select.Substring(0, 4));
            var timeKeyDate = new DateTime(year, month, 1);
            var schedules = from s in db.SchedulesOverlapping.Include(s => s.Person).Include(s => s.Workplace)
                            .Where(s => s.active == true && s.Person.DictDepartment.Name == depName &&
                                (
                                    (SqlFunctions.DatePart("month",s.DateStart) == month && SqlFunctions.DatePart("year",s.DateStart) == year)
                                    ||
                                    (SqlFunctions.DatePart("month",s.DateEnd) == month && SqlFunctions.DatePart("year",s.DateEnd) == year)
                                    ||
                                    (s.DateStart<= timeKeyDate && timeKeyDate <= s.DateEnd)
                                )
                            )
                            join d in db.DictScheduleStatus.DefaultIfEmpty()
                            on s.ScheduleStatus2 equals d.Name
                            into temp
                            from d in temp.DefaultIfEmpty()
                            select new
                            {
                                s.ScheduleId,
                                s.Person,
                                s.DateStart,
                                s.DateEnd,
                                s.ScheduleStatus2,
                                s.ScheduleDesc,
                                s.secondsoverlap,
                                colorHTML = d.ColorHTML == null ? "#FFFF92" : d.ColorHTML
                            }
                            ;
            var eventList = from e in schedules
                            select new
                            {
                                id = e.ScheduleId,
                                title = e.Person.FullName,
                                start = e.DateStart,
                                end = e.DateEnd,
                                description = e.ScheduleStatus2,
                                note = e.ScheduleDesc,
                                allDay = (SqlFunctions.DateDiff("hour", e.DateStart, e.DateEnd) > 12) ? true : false,
                                color = e.colorHTML,
                                hours = (SqlFunctions.DateDiff("hour", e.DateStart, e.DateEnd) > 12) ? SqlFunctions.DateDiff("day", e.DateStart, e.DateEnd) * 8 : (SqlFunctions.DateDiff("s", e.DateStart, e.DateEnd) - e.secondsoverlap) / (decimal)3600
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public JsonResult GetDiaryEventsReport()
        {
            //var depName = Session["UserDepName"].ToString();
            var schedules = from s in db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Where(s => s.active == true)
                            join d in db.DictScheduleStatus.DefaultIfEmpty()
                            on s.ScheduleStatus equals d.Name
                            into temp
                            from d in temp.DefaultIfEmpty()
                            select new
                            {
                                s.ScheduleId,
                                s.Person,
                                s.DateStart,
                                s.DateEnd,
                                s.ScheduleStatus
                            }
                            ;
            var eventList = from e in schedules
                            select new
                            {
                                Person = e.Person.FullName,
                                Department = e.Person.DictDepartment.Name,
                                Team = e.Person.DictTeam.Name,
                                e.Person.FormOfEmployment,
                                e.Person.TypeOfEmployment,
                                JobTitle = e.Person.DictJobTitle.Name,
                                Skill = e.Person.DictSkill.Name,
                                e.DateStart,
                                Year = SqlFunctions.DatePart("year", e.DateStart),
                                Month = SqlFunctions.DatePart("month", e.DateStart),
                                StartTime = e.DateStart,
                                HoursOfWork = e.DateStart,
                                e.DateEnd,
                                NoOfHours = (DbFunctions.DiffHours(e.DateStart, e.DateEnd) > 12) ? SqlFunctions.DateDiff("day", e.DateStart, e.DateEnd)*8 : SqlFunctions.DateDiff("hour", e.DateStart, e.DateEnd),
                                e.ScheduleStatus,
                                IsFullDay = (DbFunctions.DiffHours(e.DateStart, e.DateEnd) > 12) ? true : false
                            };
            var rows = eventList.ToArray();
            return Json(rows, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDiaryEventsPersonal()
        {
            var depName = Session["UserDepName"].ToString();
            var fullName = Session["UserFullName"].ToString();
            var schedules = from s in db.Schedules.Include(s => s.Person).Include(s => s.Workplace).Where(s => s.active == true && s.Person.DictDepartment.Name == depName && s.Person.FullName == fullName)
                            join d in db.DictScheduleStatus.DefaultIfEmpty()
                            on s.ScheduleStatus equals d.Name
                            into temp
                            from d in temp.DefaultIfEmpty()
                            select new
                            {
                                s.ScheduleId,
                                s.Person,
                                s.DateStart,
                                s.DateEnd,
                                s.ScheduleStatus,
                                colorHTML = d.ColorHTML == null ? "#FFFF92" : d.ColorHTML
                            }
                            ;
            var eventList = from e in schedules
                            select new
                            {
                                id = e.ScheduleId,
                                title = e.Person.FullName,
                                start = e.DateStart,
                                end = e.DateEnd,
                                description = e.ScheduleStatus,
                                allDay = (DbFunctions.DiffHours(e.DateStart, e.DateEnd) > 12) ? true : false,
                                color = e.colorHTML
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDaysOffs()
        {
            var depName = Session["UserDepName"].ToString();
            var depID = db.DictDepartments.Where(x => x.Name == depName).FirstOrDefault().DictDepartmentID;
            var daysOffs = db.DictDaysOffs.Where(x => x.DictDepartmentID == depID).Select(u => new { DateDayOff = u.DateDayOff }).ToArray();
            return Json(daysOffs, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetScheduleStatusAll()
        {
            return Json(db.DictScheduleStatus.Select(a => new { label = a.Name }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetScheduleStatus(string term)
        {
            return Json(db.DictScheduleStatus.Where(c => c.Name.StartsWith(term)).Select(a => new { label = a.Name }), JsonRequestBehavior.AllowGet);
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // not used yet
        public ActionResult DetailsPart(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return PartialView(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            var depName = Session["UserDepName"].ToString();

            ViewBag.PersonId = new SelectList(db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName), "PersonId", "FullName");
            ViewBag.DictTeam = new SelectList(db.DictTeams.Distinct(), "DictTeamID", "Name");
            ViewBag.Persons = db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName).Include(s => s.DictTeam);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name");
            return View();
        }
        // GET: Schedules/Create2
        public ActionResult Create2()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            var depName = Session["UserDepName"].ToString();

            ViewBag.PersonId = new SelectList(db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName), "PersonId", "FullName");
            ViewBag.DictTeam = new SelectList(db.DictTeams.Distinct(), "DictTeamID", "Name");
            ViewBag.Persons = db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName).Include(s => s.DictTeam);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name");
            return View();
        }

        // POST: Schedules/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ScheduleId,PersonId,DateStart,DateEnd,ScheduleStatus,ScheduleDesc,WorkplaceId,active")] Schedule schedule, FormCollection collection)
        {
            string[] PersonList = collection["PersonId"].Split(',');
            var repeat = collection["repeat"];
            var dateStart = schedule.DateStart;
            var dateEnd = schedule.DateEnd;
            schedule.ScheduleStatus = schedule.ScheduleStatus.First().ToString().ToUpper() + schedule.ScheduleStatus.Substring(1) ;
            if (ModelState.IsValid)
            {
                foreach (string value in PersonList)
                {
                    schedule.DateStart = dateStart;
                    schedule.DateEnd = dateEnd;
                    schedule.PersonId = Int32.Parse(value);
                    if (repeat == "daily")
                    {
                        for (int i = 0; i < Int32.Parse(collection["daily_every"]); i++)
                        {
                            db.Schedules.Add(schedule);
                            db.SaveChanges();
                            schedule.DateStart = schedule.DateStart.AddDays(1);
                            schedule.DateEnd = schedule.DateEnd.AddDays(1);
                        }
                    }
                    else if (repeat == "weekly")
                    {
                        int[] arrWeekly = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            arrWeekly[j] = (collection["weekly_" + (j + 1)] == "on") ? 1 : 0;

                        }
                        for (int i = 0; i < Int32.Parse(collection["weekly_every"]) * 7; i++)
                        {
                            var weekDay = (int)(schedule.DateStart.DayOfWeek + 6) % 7;
                            if (arrWeekly[weekDay] == 1)
                            {
                                db.Schedules.Add(schedule);
                                db.SaveChanges();
                            }
                            schedule.DateStart = schedule.DateStart.AddDays(1);
                            schedule.DateEnd = schedule.DateEnd.AddDays(1);

                        }
                    }
                    else if (repeat == "monthly")
                    {
                        int[] arrMonthly = new int[31];
                        for (int j = 0; j < 31; j++)
                        {
                            arrMonthly[j] = (collection["monthly_" + (j + 1)] == "on") ? 1 : 0;

                        }
                        for (int i = 0; i < Int32.Parse(collection["monthly_every"]); i++)
                        {
                            int days = DateTime.DaysInMonth(schedule.DateStart.Year, schedule.DateStart.Month) - schedule.DateStart.Day + 1;
                            for (int j = 0; j < days; j++)
                            {
                                if (arrMonthly[schedule.DateStart.Day - 1] == 1)
                                {
                                    db.Schedules.Add(schedule);
                                    db.SaveChanges();
                                }
                                schedule.DateStart = schedule.DateStart.AddDays(1);
                                schedule.DateEnd = schedule.DateEnd.AddDays(1);
                            }
                        }
                    }
                    else //norepeat
                    {
                        db.Schedules.Add(schedule);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "FullName", schedule.PersonId);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name", schedule.WorkplaceId);
            return View(schedule);
        }
        // POST: Schedules/Create
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "ScheduleId,PersonId,DateStart,DateEnd,ScheduleStatus,ScheduleDesc,WorkplaceId,active")] Schedule schedule, FormCollection collection)
        {
            string[] PersonList = collection["PersonId"].Split(',');
            var repeat = collection["repeat"];
            var dateStart = schedule.DateStart;
            var dateEnd = schedule.DateEnd;
            schedule.ScheduleStatus = schedule.ScheduleStatus.First().ToString().ToUpper() + schedule.ScheduleStatus.Substring(1);
            if (ModelState.IsValid)
            {
                foreach (string value in PersonList)
                {
                    schedule.DateStart = dateStart;
                    schedule.DateEnd = dateEnd;
                    schedule.PersonId = Int32.Parse(value);
                    if (repeat == "daily")
                    {
                        for (int i = 0; i < Int32.Parse(collection["daily_every"]); i++)
                        {
                            db.Schedules.Add(schedule);
                            db.SaveChanges();
                            schedule.DateStart = schedule.DateStart.AddDays(1);
                            schedule.DateEnd = schedule.DateEnd.AddDays(1);
                        }
                    }
                    else if (repeat == "weekly")
                    {
                        int[] arrWeekly = new int[7];
                        for (int j = 0; j < 7; j++)
                        {
                            arrWeekly[j] = (collection["weekly_" + (j + 1)] == "on") ? 1 : 0;

                        }
                        for (int i = 0; i < Int32.Parse(collection["weekly_every"]) * 7; i++)
                        {
                            var weekDay = (int)(schedule.DateStart.DayOfWeek + 6) % 7;
                            if (arrWeekly[weekDay] == 1)
                            {
                                db.Schedules.Add(schedule);
                                db.SaveChanges();
                            }
                            schedule.DateStart = schedule.DateStart.AddDays(1);
                            schedule.DateEnd = schedule.DateEnd.AddDays(1);

                        }
                    }
                    else if (repeat == "monthly")
                    {
                        int[] arrMonthly = new int[31];
                        for (int j = 0; j < 31; j++)
                        {
                            arrMonthly[j] = (collection["monthly_" + (j + 1)] == "on") ? 1 : 0;

                        }
                        for (int i = 0; i < Int32.Parse(collection["monthly_every"]); i++)
                        {
                            int days = DateTime.DaysInMonth(schedule.DateStart.Year, schedule.DateStart.Month) - schedule.DateStart.Day + 1;
                            for (int j = 0; j < days; j++)
                            {
                                if (arrMonthly[schedule.DateStart.Day - 1] == 1)
                                {
                                    db.Schedules.Add(schedule);
                                    db.SaveChanges();
                                }
                                schedule.DateStart = schedule.DateStart.AddDays(1);
                                schedule.DateEnd = schedule.DateEnd.AddDays(1);
                            }
                        }
                    }
                    else //norepeat
                    {
                        db.Schedules.Add(schedule);
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Table");
            }

            ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "FullName", schedule.PersonId);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name", schedule.WorkplaceId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
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
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "FullName", schedule.PersonId);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name", schedule.WorkplaceId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ScheduleId,PersonId,DateStart,DateEnd,ScheduleStatus,ScheduleDesc,WorkplaceId,active")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "FullName", schedule.PersonId);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name", schedule.WorkplaceId);
            return View(schedule);
        }

        // GET: Schedules/MoveScheduleNextPerson
        public ActionResult MoveScheduleNextPerson()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }
            var depName = Session["UserDepName"].ToString();

            

            List<SelectListItem> items = new SelectList(db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName), "PersonId", "FullName").ToList();
            items.Insert(0, (new SelectListItem { Text = "[to delete]", Value = "0" }));

            ViewBag.DictTeam = new SelectList(db.DictTeams.Distinct(), "DictTeamID", "Name");
            ViewBag.Persons = db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName).Include(s => s.DictTeam);
            ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name");
            ViewBag.PersonId = new SelectList(db.Persons.Where(s => s.active == true && s.DictDepartment.Name == depName), "PersonId", "FullName");
            ViewBag.PersonId2 = new SelectList(items, "Value", "Text", null);
 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MoveScheduleNextPerson(FormCollection collection)
        {
            string[] PersonList = collection["PersonId"].Split(',');

            if (ModelState.IsValid)
            {
                foreach (string value in PersonList)
                {
                    var PersonId = Int32.Parse(value);
                    var PersonId2 = Int32.Parse(collection["PersonId2"]);
                    var DateStart = DateTime.Parse(collection["DateStart"]);
                    var DateEnd = DateTime.Parse(collection["DateEnd"]);

                    List<Schedule> schedule = db.Schedules.Where(x => x.PersonId == PersonId && ((x.DateStart <= DateStart && x.DateEnd > DateStart) || (x.DateStart <= DateEnd && x.DateEnd > DateEnd) || (x.DateStart > DateStart && x.DateEnd < DateEnd))).ToList();

                    foreach (var item in schedule)
                    {
                        var result = db.Schedules.SingleOrDefault(x => x.ScheduleId == item.ScheduleId);

                        if (PersonId2 == 0)
                        {
                            result.active = false;
                        }
                        else
                        {
                            result.PersonId = PersonId2;
                        }
                        db.Entry(result).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                }
               

                return RedirectToAction("Index");
            }

            //ViewBag.PersonId = new SelectList(db.Persons, "PersonId", "FullName", schedule.PersonId);
            //ViewBag.WorkplaceId = new SelectList(db.Workplaces, "WorkplaceId", "Name", schedule.WorkplaceId);
            return View();
        }

        public FileResult DownloadExcel()
        {
            string path = "/Doc/WorkSchedule_temp.xlsx";
            return File(path, "application/vnd.ms-excel", "WorkSchedule_temp" + DateTime.Now  + ".xlsx");
        }

        [HttpPost]
        public ActionResult UploadExcel(Schedule schedule, HttpPostedFileBase FileUpload)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "User"))
            {
                return RedirectToAction("Index", "Home");
            }


            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {


                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    FileUpload.InputStream.Close();
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }
                    var objConn = new OleDbConnection(connectionString);
                    objConn.Open();

                    string sheetName = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0].ItemArray[2].ToString();

                    var adAPTer = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", connectionString);
                    var ds = new DataSet();

                    adAPTer.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var items = from a in excelFile.Worksheet<ScheduleXls>(sheetName.Replace("$","")) select a;

                    foreach (var a in items)
                    {
                        try
                        {
                            var person = db.Persons.Where(x => x.FullName == a.FullName).FirstOrDefault();
                            if (a.FullName != "" && a.DateStart.ToString() != "" && a.DateEnd.ToString() != "" 
                                && a.ScheduleStatus != "" && person != null)
                            {
                                var personId = db.Persons.Where(x => x.FullName == a.FullName && x.active == true).FirstOrDefault().PersonId;
                                Schedule TU = new Schedule();
                                TU.PersonId = personId;
                                TU.DateStart = RoundDate(a.DateStart);
                                TU.DateEnd = RoundDate(a.DateEnd);
                                TU.ScheduleStatus = a.ScheduleStatus;
                                TU.ScheduleDesc = a.ScheduleDesc;
                                TU.active = true;
                                db.Schedules.Add(TU);

                                db.SaveChanges();



                            }
                            else
                            {
                                data.Add("<ul>");
                                if (a.FullName == "" || a.FullName == null) data.Add("<li> Employee name is required</li>");
                                if (person == null) data.Add("<li>Employee not found with the given name</li>");
                                if (a.DateStart.ToString() == "" || a.DateStart == null) data.Add("<li> Start date is required</li>");
                                if (a.DateEnd.ToString() == "" || a.DateEnd == null) data.Add("<li>End date is required</li>");
                                if (a.ScheduleStatus == "" || a.ScheduleStatus == null) data.Add("<li>Schedule status is required</li>");
                                data.Add("</ul>");

                                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">" + string.Join("", data) + "</div>";
                                return View("ExcelImport");
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {

                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {

                                    Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                }

                            }
                        }
                    }

                    objConn.Close();
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    ViewBag.Message = @"<div class=""alert alert-success"" role=""alert"">Correctly imported</div>";
                    return View("ExcelImport");
                }
                else
                {
                    ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">Only Excel files are allowed</div>";
                    return View("ExcelImport");
                }
            }
            else
            {
                ViewBag.Message = @"<div class=""alert alert-danger"" role=""alert"">Please select the file to import</div>";
                return View("ExcelImport");
            }
        }

        public ActionResult DownloadiCal()
        {
            StringBuilder sb = new StringBuilder();
            string DateFormat = "yyyyMMddTHHmmssZ";
            string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("PRODID:-//Compnay Inc//Product Application//EN");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");
            foreach (var res in db.Schedules)
            {
                DateTime dtStart = Convert.ToDateTime(res.DateStart);
                DateTime dtEnd = Convert.ToDateTime(res.DateEnd);
                sb.AppendLine("BEGIN:VEVENT");
                sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
                sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
                sb.AppendLine("DTSTAMP:" + now);
                sb.AppendLine("UID:" + Guid.NewGuid());
                sb.AppendLine("CREATED:" + now);
                sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + res.ScheduleDesc);
                
                sb.AppendLine("LAST-MODIFIED:" + now);
                sb.AppendLine("LOCATION:" + "");
                sb.AppendLine("SEQUENCE:0");
                sb.AppendLine("STATUS:CONFIRMED");
                sb.AppendLine("SUMMARY:" + res.ScheduleStatus);
                sb.AppendLine("TRANSP:OPAQUE");
                //Alarm
                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("TRIGGER:" + String.Format("-PT{0}M", "30"));
                sb.AppendLine("REPEAT:" + "2");
                sb.AppendLine("DURATION:" + String.Format("PT{0}M", "15"));
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine("DESCRIPTION:" + "Work");
                sb.AppendLine("END:VALARM");

                sb.AppendLine("END:VEVENT");
            }
            sb.AppendLine("END:VCALENDAR");
            var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());

            return File(calendarBytes, "text/calendar", "event.ics");
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
