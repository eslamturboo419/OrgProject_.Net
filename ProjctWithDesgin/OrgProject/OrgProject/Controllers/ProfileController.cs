using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin,Doctor,Leader")]
    public class ProfileController : Controller
    {

        private MyDBEntities db = new MyDBEntities();

        public async Task<ActionResult> ViewProfile(string Email)
        {

          

            if (Email == null || ! User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "Home");
            }

            if(Email.ToLower() !=User.Identity.Name.ToLower())
            {
                return View("Error");
            }

            Email = User.Identity.Name;

            AllUserData data =await db.AllUserDatas.Where(x => x.Email == Email).FirstOrDefaultAsync();
           
            if (data == null)
            {
                return View("Error");
            }
             
        
            return View(data);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int? id )
        {
            string email = User.Identity.Name;

            if (id == null)
            {
                return View("Error");
            }
            AllUserData allUserData =await db.AllUserDatas.FirstOrDefaultAsync(x=>x.Email==email && x.id==id);
           
            if (allUserData == null)
            {
                return View("Error");
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
            
            return View(allUserData);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AllUserData allUserData)
        {
             
            if (ModelState.IsValid)
            {
                db.Entry(allUserData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ViewProfile",new { Email = allUserData.Email  });
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
            
            return View(allUserData);
        }


        // Rest password
        [HttpGet]
        public async Task<ActionResult> RestPassword(int ? id)
        {
            string email = User.Identity.Name;

            if (id == null)
            {
                 return View("Error");
            }
            var  allUserData =await db.AllUserDatas.FirstOrDefaultAsync(x=>x.id==id && x.Email== email);

            if (allUserData != null)
            {
                ResetPasswordClass model = new ResetPasswordClass();
                
                // encode id
                model.HiddenString = EncryptDecrypt.Encode(allUserData.id.ToString());

                return View(model);
            }
            else
            {
                return   View("Error");
            }
           
            

 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RestPassword(ResetPasswordClass reset)
        {
           
            if(ModelState.IsValid)
            {
                // decode id 
                reset.HiddenString = EncryptDecrypt.Decode(reset.HiddenString);
                
                //enc password
                var nnn = Crypto.Hash(reset.OldPassword.ToLower());
                var old =await db.AllUserDatas.Where(x => x.id.ToString() == reset.HiddenString && x.Password == nnn).FirstOrDefaultAsync();
                if (old==null)
                {
                    ModelState.AddModelError("ErrorPassword", "Your Old Password Is Error");
                    return View(old);
                }

                var result = db.AllUserDatas.Where(x => x.id.ToString() == reset.HiddenString).FirstOrDefault();
                if(reset ==null)
                {
                    return View();
                }
                




                var newpass = Crypto.Hash(reset.NewPassword.ToLower());
                result.Password = newpass;

                db.Configuration.ValidateOnSaveEnabled = false; // confirm password

                db.SaveChanges();
                //return RedirectToAction("ViewProfile");
                return RedirectToAction("ViewProfile", new { Email = User.Identity.Name });
            }
            return View(reset);



        }



    }
}