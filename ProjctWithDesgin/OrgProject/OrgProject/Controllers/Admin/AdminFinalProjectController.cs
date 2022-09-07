using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OrgProject.Models;
using System.Threading.Tasks;
using PagedList;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFinalProjectController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        public ActionResult Dash()
        {
            TempData["TotalUser"] = db.AllUserDatas.Count();
            TempData["TotalProject"] = db.FinalProjects.Count();
            TempData["TotalMaster"] = db.FinalMasters.Count();
            TempData["TotalComment"] = db.Comments.Count();


            return View();
        }


        ////public async Task<ActionResult> Index()
        ////{
        ////    return View(await db.FinalProjects.OrderByDescending(x => x.id).ToListAsync());
        ////}
        public ActionResult Index(string Search, int? Page)
        {
            var val = db.FinalProjects.
                Where(x=> x.NameOfProject.StartsWith(Search) || (x.NameOfProject.Contains(Search)) || Search == null).OrderByDescending(x=>x.YearOfTheProject);

            return View( val.ToList().ToPagedList(Page ?? 1, 10)) ;
        }


        [HttpGet]
        public ActionResult Create()
        {

            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");
           // ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x=>x.Type.Type1== "TeachAss").Select(x=>x.id), "id", "F_Name");


            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name");
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");

            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase PdfFile
            , HttpPostedFileBase ImageProject,
            HttpPostedFileBase VideoUpload,
            FinalProject finalProject)
        {

            string extentionPDF = "";
            string extentionImage = "";
            string extentionVideo = "";

            if (ModelState.IsValid)
            {
                if (finalProject.PdfFile != null)
                {
                    extentionPDF = Path.GetExtension(PdfFile.FileName);

                    if (extentionPDF.ToLower().Equals(".pdf") )
                    {
                        
                        // save PDF in folder
                        string PDFName = System.IO.Path.GetFileName(PdfFile.FileName);
                        string PDFName2 = DateTime.Now.ToString("yymmss") + PDFName;
                        string physicalPath = Server.MapPath("~/PdfImageVideo/PDFFOLDER/" + PDFName2);
                        PdfFile.SaveAs(physicalPath);
                        finalProject.PdfFile = PDFName2;
                    }
                    else
                    {
                        ModelState.AddModelError("PDFError", "Error Extention must be '.pdf ' ");
                        //  ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
                        // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);

                        ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                    Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                                Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);

                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);
                        return View(finalProject);
                    }


                
                }

                if (finalProject.ImageProject != null)
                {
                    ///save image in folder

                    extentionImage = Path.GetExtension(ImageProject.FileName);

                    if (extentionImage.ToLower().Equals(".jpg") || extentionImage.ToLower().Equals(".jpg") || extentionImage.ToLower().Equals(".jpeg"))
                    { 
                        // save image
                        string imgName = System.IO.Path.GetFileName(ImageProject.FileName);
                        string imgName2 = DateTime.Now.ToString("yymmss") + imgName;
                        string physicalPathimg = Server.MapPath("~/PdfImageVideo/images/" + imgName2);
                        ImageProject.SaveAs(physicalPathimg);
                        finalProject.ImageProject = imgName2;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageError", "Error extention Must be ' .jpg or .jpg or .jpeg'  ");

                        //ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
                        //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);


                        ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                         Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                                Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);



                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);
                        return View(finalProject);
                    }


                }


                if (finalProject.VideoUpload != null)
                {
                    /// video upload
                    /// 
                    extentionVideo = Path.GetExtension(VideoUpload.FileName);
                    if (extentionVideo.ToLower().Equals(".mp4") )
                    {
                        string VideoName = System.IO.Path.GetFileName(VideoUpload.FileName);
                        string VideoName2 = DateTime.Now.ToString("yymmss") + VideoName;
                        string physicalPathvideo = Server.MapPath("~/PdfImageVideo/VideoFolder/" + VideoName2);
                        VideoUpload.SaveAs(physicalPathvideo);
                        finalProject.VideoUpload = VideoName2;
                    }
                    else
                    {
                        ModelState.AddModelError("VideoError", "Error extention Must be ' .mp4' please convert video to '.mp4'  ");

                        //ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
                        //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);
                        ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                                            Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                                Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);

                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);
                        return View(finalProject);
                    }
                     
                }


                db.FinalProjects.Add(finalProject);
                await  db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");

            }

            // ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);

            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                         Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                    Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);



            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);
            return View(finalProject);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            FinalProject finalProject =await db.FinalProjects.FindAsync(id);
            if (finalProject == null)
            {
                return View("Error");
            }
            // ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
            // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);

            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                      Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                    Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);


            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);
            return View(finalProject);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( FinalProject finalProject, int YearOfTheProject)
        {
         

            if (ModelState.IsValid)
            {
                finalProject.YearOfTheProject = YearOfTheProject;
                db.Entry(finalProject).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            // ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", finalProject.TeachAss_id);
            // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalProject.Doc_id);

            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
                                       Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.TeachAss_id);

            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                    Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalProject.Doc_id);


            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalProject.Dep_id);


            return View(finalProject);
        }

        public JsonResult DeleteEmployee(int ? EmployeeId)
        {
            bool result = false;
            if (EmployeeId == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            FinalProject s = db.FinalProjects.Where(x => x.id == EmployeeId).SingleOrDefault();
            if (s != null)
            {

                string fullpathimg = Request.MapPath("~/PdfImageVideo/images/" + s.ImageProject);
                if (System.IO.File.Exists(fullpathimg))
                {
                    System.IO.File.Delete(fullpathimg);
                }
                string fullpathPdf = Request.MapPath("~/PdfImageVideo/PDFFOLDER/" + s.PdfFile);
                if (System.IO.File.Exists(fullpathPdf))
                {
                    System.IO.File.Delete(fullpathPdf);
                }

                string fullpathvideo = Request.MapPath("~/PdfImageVideo/VideoFolder/" + s.VideoUpload);
                if (System.IO.File.Exists(fullpathvideo))
                {
                    System.IO.File.Delete(fullpathvideo);
                }



                db.FinalProjects.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }






    }
}