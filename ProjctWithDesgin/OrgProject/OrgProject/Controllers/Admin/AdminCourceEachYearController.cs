using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;
using PagedList;

namespace OrgProject.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminCourceEachYearController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        
        public   ActionResult Index(string Search, int? Page)
        {
            var val = db.CoursesWithYears.Where(x => x.NameOfCource.StartsWith(Search) || (x.NameOfCource.Contains(Search)) || Search == null).OrderByDescending(x => x.id);


            return View(val.ToList().ToPagedList(Page ?? 1, 10));

        }

        [HttpGet]
        public ActionResult Create()
        {
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name");
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor" || x.Type.Type1 == "TeachAss").
              Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");
            ViewBag.Deaprtment = new SelectList(db.Departement_Name, "Name", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CoursesWithYear CoursesWithYear)
        {
            if (ModelState.IsValid)
            {
                CoursesWithYear.Term = CoursesWithYear.Term;

                db.CoursesWithYears.Add(CoursesWithYear);
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");

            }


            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), CoursesWithYear.Doc_id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor" || x.Type.Type1 == "TeachAss").
                     Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName" , CoursesWithYear.Doc_id);

            ViewBag.Deaprtment = new SelectList(db.Departement_Name, "Name", "Name", CoursesWithYear.Deaprtment);
            return View(CoursesWithYear);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return View("Error");
            }
            CoursesWithYear CoursesWithYear =await db.CoursesWithYears.FindAsync(id);
            if (CoursesWithYear == null)
            {
                return View("Error");
            }
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", CoursesWithYear.Doc_id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor" || x.Type.Type1 == "TeachAss").
             Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", CoursesWithYear.Doc_id);

            ViewBag.Deaprtment = new SelectList(db.Departement_Name, "Name", "Name", CoursesWithYear.Deaprtment);
            return View(CoursesWithYear);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CoursesWithYear CoursesWithYear, int YearOfThisCource)
        {
            if (ModelState.IsValid)
            {
                CoursesWithYear.YearOfThisCource = YearOfThisCource;
                db.Entry(CoursesWithYear).State = EntityState.Modified;
               await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", CoursesWithYear.Doc_id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor" || x.Type.Type1 == "TeachAss").
             Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", CoursesWithYear.Doc_id);

            ViewBag.Deaprtment = new SelectList(db.Departement_Name, "Name", "Name", CoursesWithYear.Deaprtment);

            return View(CoursesWithYear);
        }

        public JsonResult DeleteEmployee(int ? EmployeeId)
        {
            bool result = false;
            if (EmployeeId == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            CoursesWithYear s = db.CoursesWithYears.Where(x => x.id == EmployeeId).SingleOrDefault();
            if (s != null)
            {
               
                db.CoursesWithYears.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // for autocomplete
        public JsonResult GetAllData(string term)  // must the name is " term" because jquery 
        {
            List<string> lis = db.CoursesWithYears.Where(x => x.NameOfCource.StartsWith(term)).Select(y => y.NameOfCource).Distinct().ToList();

            return Json(lis, JsonRequestBehavior.AllowGet);

        }




    }
}