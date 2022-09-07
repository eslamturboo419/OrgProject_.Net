using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminGuideController : Controller
    {
        private MyDBEntities db = new MyDBEntities();


        public async Task<ActionResult> Index()
        {
            return View(await db.GuideYears.OrderByDescending(x => x.id).ToListAsync());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( GuideYear guideYear)
        {
            if (ModelState.IsValid)
            {
                db.GuideYears.Add(guideYear);
                await  db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            return View(guideYear);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            GuideYear guideYear =await db.GuideYears.FindAsync(id);
            if (guideYear == null)
            {
                return View("Error");
            }
            return View(guideYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GuideYear guideYear, int HeaderOFYear) 
        {
            if (ModelState.IsValid)
            {
                guideYear.HeaderOFYear =HeaderOFYear;
                db.Entry(guideYear).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            return View(guideYear);
        }



        public JsonResult DeleteEmployee(int  ? EmployeeId)
        {
            bool result = false;
            if (EmployeeId == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            GuideYear s = db.GuideYears.Where(x => x.id == EmployeeId).SingleOrDefault();
            if (s != null)
            {

                db.GuideYears.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }





    }
}