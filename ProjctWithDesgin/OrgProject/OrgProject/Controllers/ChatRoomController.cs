using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin,Doctor,Leader")]
    public class ChatRoomController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        [HttpGet]
        public ActionResult Index()
        {

         string   Email = User.Identity.Name;

            var val = db.AllUserDatas.Where(x => x.Email.Equals(Email)).FirstOrDefault();
            if(val ==null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("About", "ChatRoom", new { UserNames = val.F_Name, EmailIDs = val.Email });
        }

         
        public ActionResult About(string UserNames, string EmailIDs)
        {
            
            if (UserNames == null || EmailIDs == null || string.IsNullOrEmpty(UserNames) || string.IsNullOrEmpty(EmailIDs))
            {
               return RedirectToAction("Login", "Account");
            }
            ViewBag.UserNames = UserNames;
            ViewBag.EmailIDs = EmailIDs;
            return View();
        }


    }
}