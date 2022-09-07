using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDepartmentController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.Departement_Name.OrderByDescending(x => x.Id).ToListAsync());
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")]  Departement_Name dept)
        {
            if (ModelState.IsValid)
            {
                db.Departement_Name.Add(dept);
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            return View(dept);
        }

        ///// Edit
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                
                return View("Error");
            }
            Departement_Name dept =await db.Departement_Name.FindAsync(id);
            if (dept == null)
            {
                return   View("Error");
            }
            return View(dept);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Departement_Name dept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dept).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            return View(dept);
        }


        public JsonResult DeleteDepartment(int ? DepID)
        {
            bool result = false;
            if (DepID ==null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);  
            }
           
            Departement_Name s = db.Departement_Name.Where(x => x.Id == DepID).SingleOrDefault();
            if (s != null)
            {
                // on All Delete we make Cascade
                db.Departement_Name.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }










    }
}