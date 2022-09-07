using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminManageQuestionsController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public async Task<ActionResult> Index()
        {

            return View(await db.Questions.OrderByDescending(x => x.Id).ToListAsync());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Course_id = new SelectList(db.CoursesWithYears, "id", "NameOfCource");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                await  db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            ViewBag.Course_id = new SelectList(db.CoursesWithYears, "id", "NameOfCource", question.Course_id);
            return View(question);
        }

       [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Question question =await db.Questions.FindAsync(id);
            if (question == null)
            {
                return View("Error");
            }
            ViewBag.Course_id = new SelectList(db.CoursesWithYears, "id", "NameOfCource", question.Course_id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( Question question , string Answer_Correct)
        {
            if (ModelState.IsValid)
            {
                question.Answer_Correct = Answer_Correct;

                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edit OK";
                return RedirectToAction("Index");
            }
            ViewBag.Course_id = new SelectList(db.CoursesWithYears, "id", "NameOfCource", question.Course_id);
            return View(question);
        }


        public JsonResult DeleteEmployee(int ? EmployeeId)
        {
            bool result = false;

            if (EmployeeId ==null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            Question s = db.Questions.Where(x => x.Id == EmployeeId).SingleOrDefault();
            if (s != null)
            {
                db.Questions.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}