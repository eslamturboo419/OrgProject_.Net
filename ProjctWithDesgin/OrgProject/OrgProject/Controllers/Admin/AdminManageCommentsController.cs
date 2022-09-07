using System;
using System.Collections.Generic;
using System.Data;
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
    public class AdminManageCommentsController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        // GET: AdminManageComments
        public async Task<ActionResult> Index()
        {

            return View(await db.Comments.OrderByDescending(x => x.Id).ToListAsync());
        }

         [HttpGet]
        public async Task<ActionResult> SeeAllReplay(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            var repl =await db.Replies.Where(x => x.Comment_id == id).OrderByDescending(x => x.Id).ToListAsync();

            if(repl ==null)
            {
                return View("Error");
            }
            return View(repl);
        }

        // delete comment and all its replay
        public JsonResult DeleteDepartment(int ? DepID)
        {
            bool result = false;
            if (DepID == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var s = db.Comments.Where(x => x.Id == DepID).SingleOrDefault();

            if (s != null)
            {
                db.Comments.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        // delete one Replay
        public JsonResult DeleteReplay(int ? DepID)
        {
            bool result = false;

            if (DepID == null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var s = db.Replies.Where(x => x.Id == DepID).SingleOrDefault();

            if (s != null)
            {
                db.Replies.Remove(s);
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
