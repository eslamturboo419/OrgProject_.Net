using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Leader")]
    public class TeamsController : Controller
    {
        private MyDBEntities db = new MyDBEntities();
       
        
        // GET: Teams
        public ActionResult Index()
        {
            var thisStudent = User.Identity.Name;

            var y = db.ProjectToSubmitDoctors.Where(x => x.ThisStudent ==thisStudent).FirstOrDefault();
            
            if (y != null)
            {
                if (y.IsActive == true)
                {
                    ViewBag.message = "OK True";
                }
                else if (y.IsActive == false)
                {
                    ViewBag.message = " False ";
                }
            }

            return View();
        }

        ////// submit to doctor
        [HttpGet]
        public ActionResult Submit()
        {
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");

            //ViewBag.Doc_Id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name");
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
           Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");

            //ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name");
            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
        Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(  ProjectToSubmitDoctor projectToSubmitDoctor)
        {
            if (ModelState.IsValid)
            {
                // get the username
                projectToSubmitDoctor.ThisStudent = User.Identity.Name;

                projectToSubmitDoctor.IsActive = false;
                db.ProjectToSubmitDoctors.Add(projectToSubmitDoctor);
                db.SaveChanges();
                return RedirectToAction("index");

            }


            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", projectToSubmitDoctor.Dep_id);
            //ViewBag.Doc_Id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name", projectToSubmitDoctor.Doc_Id);
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
           Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", projectToSubmitDoctor.Doc_Id);
            
            //ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name", projectToSubmitDoctor.TeachAss_id);
            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
             Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName", projectToSubmitDoctor.TeachAss_id);

            return View(projectToSubmitDoctor);
        }







        ///  submit final projct
        [HttpGet]
        public ActionResult sumitFinal()
        {
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
       


            //ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss"), "id", "F_Name");
            ViewBag.TeachAss_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "TeachAss").
              Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");

            // ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor"), "id", "F_Name");
            ViewBag.Doc_id = new SelectList(db.AllUserDatas.Where(x => x.Type.Type1 == "Doctor").
           Select(entity => new { id = entity.id, FullName = entity.F_Name + " " + entity.L_Name }).ToList(), "id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> sumitFinal(HttpPostedFileBase PdfFile
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

                    if (extentionPDF.ToLower().Equals(".pdf"))
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
                        ModelState.AddModelError("PDFErrorUser", "Error Extention must be '.pdf ' ");
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
                        ModelState.AddModelError("ImageErrorUser", "Error extention Must be ' .jpg or .jpg or .jpeg'  ");

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
                    if (extentionVideo.ToLower().Equals(".mp4"))
                    {
                        string VideoName = System.IO.Path.GetFileName(VideoUpload.FileName);
                        string VideoName2 = DateTime.Now.ToString("yymmss") + VideoName;
                        string physicalPathvideo = Server.MapPath("~/PdfImageVideo/VideoFolder/" + VideoName2);
                        VideoUpload.SaveAs(physicalPathvideo);
                        finalProject.VideoUpload = VideoName2;
                    }
                    else
                    {
                        ModelState.AddModelError("VideoErrorUser", "Error extention Must be ' .mp4' please convert video to '.mp4'  ");

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

                finalProject.IsActive = false;
                db.FinalProjects.Add(finalProject);
                await db.SaveChangesAsync();
               
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










    }
}