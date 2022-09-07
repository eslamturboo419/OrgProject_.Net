using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        [HttpGet]
        public ActionResult login()
        {
       
            ///////////////////////  all password
            //           Admin
            //            Doctor
            //            Leader
            //            TeachAss
            //          123456789   // Any Email



            if (User.Identity.IsAuthenticated)
            {
                
                return RedirectToAction("index", "home");
                 
            }

                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult login(LoginVM tB)
        {
            if(ModelState.IsValid)
            {
              
                tB.Password = Crypto.Hash(tB.Password.ToLower());
                var count = db.AllUserDatas.Where(x => x.Email.Equals(tB.Email) && x.Password.Equals(tB.Password)).FirstOrDefault();

                if (count == null)
                {
                    ViewBag.message = "Invalid Email Or Password";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(tB.Email, tB.RememberMe);

                    if(count.Type.Type1== "Admin")
                    {
                        return RedirectToAction("Dash", "AdminFinalProject");
                    }
                    if (count.Type.Type1 == "Leader")
                    {
                        return RedirectToAction("Index", "Teams");
                    }
                    if (count.Type.Type1 == "Doctor")
                    {
                        return RedirectToAction("Index", "Doctors");
                    }

                    //return RedirectToAction("index", "Home");
                }

            }
            return View(tB);

        }


        public ActionResult logOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("index", "home");
            }
            return RedirectToAction("index", "home");
        }


    }
}