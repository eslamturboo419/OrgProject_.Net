using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;
using System.Net;
using System.Data.Entity;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorsController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public ActionResult Index()
        {
            // to get the DOC_ID from table AllUserData by his Email
            var y = db.AllUserDatas.Where(x => x.Email == User.Identity.Name).FirstOrDefault();
    

           var projectToSubmitDoctors = db.ProjectToSubmitDoctors
                .Include(p => p.AllUserData).Include(p => p.AllUserData1)
                .Include(p => p.Departement_Name)
            .Where(x => x.Doc_Id==y.id);
            return View(projectToSubmitDoctors.ToList());


        }
        
        [HttpGet]
        public ActionResult Edit(int? id)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectToSubmitDoctor projectToSubmitDoctor = db.ProjectToSubmitDoctors.Find(id);

            if (projectToSubmitDoctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", projectToSubmitDoctor.Dep_id);
            ViewBag.Doc_Id = new SelectList(db.AllUserDatas.Where(x=>x.Type.Type1=="Doctor"), "id", "F_Name", projectToSubmitDoctor.Doc_Id);
            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", projectToSubmitDoctor.TeachAss_id);
            return View(projectToSubmitDoctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ProjectToSubmitDoctor projectToSubmitDoctor, string ThisStudent)
        {
            if (ModelState.IsValid)
            {

                projectToSubmitDoctor.ThisStudent = ThisStudent;
                db.Entry(projectToSubmitDoctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", projectToSubmitDoctor.Dep_id);
           ViewBag.Doc_Id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", projectToSubmitDoctor.Doc_Id);
            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", projectToSubmitDoctor.TeachAss_id);
            return View(projectToSubmitDoctor);
        }



        public JsonResult DeleteEmployee(int EmployeeId)
        {
            bool result = false;
            ProjectToSubmitDoctor s = db.ProjectToSubmitDoctors.Where(x => x.id == EmployeeId).SingleOrDefault();
            if (s != null)
            {
                // on All Delete we make Cascade
                db.ProjectToSubmitDoctors.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }




    }
}