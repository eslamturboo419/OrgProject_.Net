using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using OrgProject.Models;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;

namespace OrgProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {

        private MyDBEntities db = new MyDBEntities();


 
        // view projects
        public ActionResult Index(string Search, int? Page, string Dep_id, string YearBag)   // , string  YearThis 
        {
 
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
            ViewBag.YearBag = new SelectList(db.FinalProjects.Select(yy => yy.YearOfTheProject).Distinct());


            // for just AutoComplete Search
            if (YearBag == "" && Dep_id == "")
            {
                var val2 = db.FinalProjects.Where(xx => (xx.IsActive == true)
            && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null));
                return View(val2.ToList().ToPagedList(Page ?? 1, 9));  //
            }

            // for just department
            if (YearBag == "" && Dep_id != "")
            {
                var val3 = db.FinalProjects.Where(xx => (xx.IsActive == true)
              && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
              && (xx.Dep_id.ToString() == Dep_id)
              ).OrderBy(x => Guid.NewGuid());
                return View(val3.ToList().ToPagedList(Page ?? 1, 9));  //
            }


            // for just Year
            if (YearBag != "" && Dep_id == "")
            {
                var val4 = db.FinalProjects.Where(xx => (xx.IsActive == true)
            && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
              && (xx.YearOfTheProject.ToString() == YearBag)
              ).OrderBy(x => Guid.NewGuid());
                return View(val4.ToList().ToPagedList(Page ?? 1, 9));  //
            }





            // for if  there are data in  ( search , year , department )
            var val = db.FinalProjects.Where(xx => (xx.IsActive == true)
            && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
            && (xx.YearOfTheProject.ToString() == YearBag || YearBag == null)
            && (xx.Dep_id.ToString() == Dep_id || Dep_id == null)
            ).OrderByDescending(x => x.YearOfTheProject).OrderBy(x=>Guid.NewGuid());

            return View(val.ToList().ToPagedList(Page ?? 1, 9));


        }




        //// view projects
        //public ActionResult Index(string Search, int? Page  , string Dep_id , string YearBag)   // , string  YearThis 
        //{

            

        //    ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
        //    ViewBag.YearBag = new SelectList(db.FinalProjects.Select(yy => yy.YearOfTheProject).Distinct());


        //    // for just AutoComplete Search
        //    if (YearBag == "" && Dep_id == "")
        //    {
        //        var val2 = db.FinalProjects.Where(xx => (xx.IsActive == true)
        //    && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null) );
        //        return View(val2.ToList().ToPagedList(Page ?? 1, 9));  //
        //    }

        //    // for just department
        //    if (YearBag == "" && Dep_id != "")
        //    {
        //        var val3 = db.FinalProjects.Where(xx => (xx.IsActive == true)
        //      && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
        //      && (xx.Dep_id.ToString() ==  Dep_id ) 
        //      );
        //        return View(val3.ToList().ToPagedList(Page ?? 1, 9));  //
        //    }


        //    // for just Year
        //    if (YearBag != "" && Dep_id == "")
        //    {
        //        var val4 = db.FinalProjects.Where(xx => (xx.IsActive == true)
        //    && (xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
        //      && (xx.YearOfTheProject.ToString() == YearBag)
        //      );
        //        return View(val4.ToList().ToPagedList(Page ?? 1, 9));  //
        //    }





        //    // for if  there are data in  ( search , year , department )
        //    var val = db.FinalProjects.Where(xx => (xx.IsActive == true)
        //    &&(xx.NameOfProject.StartsWith(Search) || (xx.NameOfProject.Contains(Search)) || Search == null)
        //    &&(xx.YearOfTheProject.ToString()==YearBag || YearBag==null )
        //    &&(xx.Dep_id.ToString()==Dep_id || Dep_id==null)
        //    ).OrderByDescending(x => x.YearOfTheProject);

        //    return View(val.ToList().ToPagedList(Page ?? 1, 9) );  


        //}

        /// view details for final project
        [HttpGet]
        public async Task<ActionResult> Details(int? id)
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

            TempData["commentsFinalProject"] = db.Comments.Where(x => x.Project_Id == id).OrderByDescending(x=>x.CreatedOn).ToList();

            return View(finalProject);
        }
        // PostComment for final project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostComment(string CommentText, string ProjectID)  /*async Task<ActionResult> */
        {
            //if (Session["user_id"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            Comment comment = new Comment();
            comment.Text = CommentText;
            comment.Project_Id = Convert.ToInt32(ProjectID);
            comment.CreatedOn = DateTime.Now;

            db.Comments.Add(comment);
            //await db.SaveChangesAsync();
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());

        }
        // Post Reply for final project
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostReply(ReplyVM replyVM)
        {

            Reply replay = new Reply();
            replay.Text = replyVM.Reply;
            replay.Comment_id = replyVM.CommentID;
            replay.Project_Id = replyVM.ProjectID;
            replay.CreatedOn = DateTime.Now;

            db.Replies.Add(replay);
            await  db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());

        }


















        // view master
        public ActionResult ViewMaster(string Search, int? page,string Dep_id , string YearBag)
        {

             
            


            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
            ViewBag.YearBag = new SelectList(db.FinalProjects.Select(yy => yy.YearOfTheProject).Distinct());



            var x = db.FinalMasters.Where(xx => (xx.IsActive == true)
            && (xx.NameOfMaster.StartsWith(Search) || (xx.NameOfMaster.Contains(Search)) || Search == null)
             && (xx.YearThisMaster.ToString() == YearBag || YearBag == null)
             && (xx.Dep_id.ToString() == Dep_id || Dep_id == null)
            ).OrderByDescending(xx=> xx.YearThisMaster);
            return View(x.ToList().ToPagedList(page ?? 1, 8));
        }

        /// view details for final master
        [HttpGet]
        public async Task<ActionResult> DetailsMaster(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            FinalMaster finalProject =await db.FinalMasters.FindAsync(id);
            if (finalProject == null)
            {
                return View("Error");
            }
            TempData["CommentsFinalMaster"] = db.Comments.Where(x => x.Master_id == id).OrderByDescending(x => x.CreatedOn).ToList();

            return View(finalProject);
        }
        // PostComment for final master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostCommentMaster(string CommentText, string ProjectID)
        {
            //if (Session["user_id"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            Comment comment = new Comment();
            comment.Text = CommentText;
            comment.Master_id = Convert.ToInt32(ProjectID);
            comment.CreatedOn = DateTime.Now;

            db.Comments.Add(comment);
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());

        }
        // Post Reply for final master
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostReplyMaster(ReplyVM replyVM)
        {

            Reply replay = new Reply();
            replay.Text = replyVM.Reply;
            replay.Comment_id = replyVM.CommentID;
            replay.Master_id = replyVM.ProjectID;
            replay.CreatedOn = DateTime.Now;

            db.Replies.Add(replay);
            await db.SaveChangesAsync();
            return Redirect(Request.UrlReferrer.ToString());

        }










   

        //  view cource with year
        [HttpGet]
        public  ActionResult ViewCourceWithYear(int year, string dep = null)
        {


            // to get Guide 
            var val = db.GuideYears.Where(m => m.HeaderOFYear == year).ToList();
            if (val != null)
            {
                TempData["DataGuide"] = val;
            }

            // to get the course with year
            if (dep == null || dep == "" || string.IsNullOrEmpty(dep))
            {
                var x = db.CoursesWithYears.Where(xz => xz.YearOfThisCource == year);
                if (x == null)
                {
                    return View("Error");
                 
                }
             

                return View(x.ToList());
       
            }


            var newx = db.CoursesWithYears.Where(xz => xz.YearOfThisCource == year && xz.Deaprtment == dep);
            if(newx==null)
            {
                return View("Error");
            }
      

            return View(newx.ToList());
  
            //TempData["Articale"] = db.GuideYears.Select(y=>y.Article);



        }
 


        // for auto complete "Final Project"
        public JsonResult GetAllData(string term)  // must the name is " term" because jquery 
        {
            List<string> lis = db.FinalProjects.Where(x => x.NameOfProject.StartsWith(term)).Select(y => y.NameOfProject).Distinct().ToList();

            return Json(lis, JsonRequestBehavior.AllowGet);

        }

        // for auto complete "Final Master"
        public JsonResult GetAllDataMaster(string term)  // must the name is " term" because jquery 
        {
            List<string> lis = db.FinalMasters.Where(x => x.NameOfMaster.StartsWith(term)).Select(y => y.NameOfMaster).Distinct().ToList();

            return Json(lis, JsonRequestBehavior.AllowGet);

        }



        // language changes
        public ActionResult Change(string LangVal)
        {
            if (LangVal != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LangVal);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LangVal);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LangVal;
            Response.Cookies.Add(cookie);
            Thread.Sleep(300);
            return RedirectToAction("index");
        }




    }
}