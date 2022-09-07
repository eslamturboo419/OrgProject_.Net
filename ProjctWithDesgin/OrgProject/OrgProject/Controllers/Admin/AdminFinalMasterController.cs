using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;
using PagedList;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminFinalMasterController : Controller
    {
        private MyDBEntities db = new MyDBEntities();


        public  ActionResult Index(string Search, int? Page)
        {
            var val = db.FinalMasters.
            Where(x => x.NameOfMaster.StartsWith(Search) || (x.NameOfMaster.Contains(Search)) || Search == null)
                .OrderByDescending(x => x.id);

            return View(val.ToList().ToPagedList(Page ?? 1, 10));
        }

        [HttpGet]
        public ActionResult Create()
        {

            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name");
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
            Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase PdfFile
            , HttpPostedFileBase ImageProject
            //, HttpPostedFileBase VideoUpload
            ,FinalMaster finalMaster)
        {


            string extentionPDF = "";
            string extentionImage = "";
            //string extentionVideo = "";


            if (ModelState.IsValid)
            {

                /// pdf
                if (finalMaster.PdfFile != null)
                {
                    extentionPDF = Path.GetExtension(PdfFile.FileName);

                    if (extentionPDF.ToLower().Equals(".pdf"))
                    {
                        //pdf file
                        string PDFName = System.IO.Path.GetFileName(PdfFile.FileName);
                        string PDFName2 = DateTime.Now.ToString("yymmss") + PDFName;
                        string physicalPath = Server.MapPath("~/PdfImageVideo/PDFFOLDER/" + PDFName2);
                        // save PDF in folder
                        PdfFile.SaveAs(physicalPath);
                        finalMaster.PdfFile = PDFName2;
                    }
                    else
                    {
                        ModelState.AddModelError("PDFError", "Error Extention must be '.pdf ' ");
                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
                       // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);
                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
                        Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName",finalMaster.Doc_id);


                        return View(finalMaster);
                    }
               

                }





                /// image
                if (finalMaster.ImageProject != null)
                {
                    extentionImage = Path.GetExtension(ImageProject.FileName);

                    if (extentionImage.ToLower().Equals(".jpg") || extentionImage.ToLower().Equals(".jpg") || extentionImage.ToLower().Equals(".jpeg"))
                    {
                        /// save image
                        string imgName = System.IO.Path.GetFileName(ImageProject.FileName);
                        string imgName2 = DateTime.Now.ToString("yymmss") + imgName;
                        string physicalPathimg = Server.MapPath("~/PdfImageVideo/images/" + imgName2);
                        ImageProject.SaveAs(physicalPathimg);
                        finalMaster.ImageProject = imgName2;
                    }
                    else
                    {
                        ModelState.AddModelError("ImageError", "Error extention Must be ' .jpg or .jpg or .jpeg'  ");
                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
                        // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);

                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
            Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName",finalMaster.Doc_id);


                        return View(finalMaster);
                    }

                     
                }



               /* if (finalMaster.VideoUpload != null)
                {
                    extentionVideo = Path.GetExtension(VideoUpload.FileName);
                    if (extentionVideo.ToLower().Equals(".mp4"))
                    {
                        /// video upload
                        string VideoName = System.IO.Path.GetFileName(VideoUpload.FileName);
                        string VideoName2 = DateTime.Now.ToString("yymmss") + VideoName;
                        string physicalPathvideo = Server.MapPath("~/PdfImageVideo/VideoFolder/" + VideoName2);
                        VideoUpload.SaveAs(physicalPathvideo);
                        finalMaster.VideoUpload = VideoName2;
                    }
                    else
                    {
                        ModelState.AddModelError("VideoError", "Error extention Must be ' .mp4' please convert video to '.mp4'  ");
                        ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
                        ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);
                        return View(finalMaster);
                    }
                     
                }*/



                db.FinalMasters.Add(finalMaster);
                await  db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
           // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
         Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalMaster.Doc_id);


            return View(finalMaster);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            FinalMaster finalMaster =await db.FinalMasters.FindAsync(id);
            if (finalMaster == null)
            {
                return View("Error");
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
         Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalMaster.Doc_id);

            return View(finalMaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FinalMaster finalMaster, int YearThisMaster)
        {
            if (ModelState.IsValid)
            {
                finalMaster.YearThisMaster = YearThisMaster;
                db.Entry(finalMaster).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", finalMaster.Dep_id);
            //ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", finalMaster.Doc_id);

            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
         Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", finalMaster.Doc_id);


            return View(finalMaster);
        }



        // delete
        public JsonResult DeleteEmployee(int  ? EmployeeId)
        {
            bool result = false;
            if (EmployeeId == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            FinalMaster s = db.FinalMasters.Where(x => x.id == EmployeeId).SingleOrDefault();
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

                //string fullpathvideo = Request.MapPath("~/PdfImageVideo/VideoFolder/" + s.VideoUpload);
                //if (System.IO.File.Exists(fullpathvideo))
                //{
                //    System.IO.File.Delete(fullpathvideo);
                //}

                db.FinalMasters.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }






    }
}