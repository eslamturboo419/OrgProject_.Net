using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;
using PagedList;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminManageUsersController : Controller
    {

        private MyDBEntities db = new MyDBEntities();

        public ActionResult Index(string Search, int? Page)
        {
            var em = User.Identity.Name;

            var val = db.AllUserDatas.Where(x => x.Email != em &&( x.F_Name.StartsWith(Search) || Search==null ) ).OrderByDescending(x => x.id);
           

            return View(val.ToList().ToPagedList(Page ?? 1, 10) );
        }

       

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name");
            ViewBag.type_id = new SelectList(db.Types, "id", "Type1");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AllUserData allUserData)
        {
            if (ModelState.IsValid)
            {

                var isexist = IsEmailExist((allUserData.Email).ToString());
                if (isexist)
                {
                    ModelState.AddModelError("emailexit", "Email is Alerdy Exist");
                    ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
                    ViewBag.type_id = new SelectList(db.Types, "id", "Type1", allUserData.type_id);
                    return View(allUserData);
                }

                //// send to gmail
               SendPasswordToGmail(allUserData.Email, allUserData.F_Name, allUserData.Password);

                // hash password with ToLower() 
                allUserData.Password = Crypto.Hash(allUserData.Password.ToLower());

                db.AllUserDatas.Add(allUserData);
                await  db.SaveChangesAsync();

           
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
            ViewBag.type_id = new SelectList(db.Types, "id", "Type1", allUserData.type_id);

           
            return View(allUserData);
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            AllUserData allUserData =await db.AllUserDatas.FindAsync(id);
            if (allUserData == null)
            {
                return View("Error");
            }
            
            TempData["pass"] = allUserData.Password;
            TempData.Keep();

            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
            ViewBag.type_id = new SelectList(db.Types, "id", "Type1", allUserData.type_id);
            allUserData.Password = "";
            return View(allUserData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AllUserData allUserData)
        {
         
            if (ModelState.IsValid)
            {
                // pass
                if (allUserData.Password == null || string.IsNullOrEmpty(allUserData.Password) || allUserData.Password == "")
                {
                    allUserData.Password = TempData["pass"].ToString();
                }

                db.Entry(allUserData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Edited OK";
                return RedirectToAction("Index");
            }
            ViewBag.Dep_id = new SelectList(db.Departement_Name, "Id", "Name", allUserData.Dep_id);
            ViewBag.type_id = new SelectList(db.Types, "id", "Type1", allUserData.type_id);
            return View(allUserData);
        }


        public JsonResult DeleteEmployee(int  ? EmployeeId)
        {
            bool result = false;
            if (EmployeeId !=null)
            {
                
                AllUserData s = db.AllUserDatas.Where(x => x.id == EmployeeId).SingleOrDefault();
                if (s != null)
                {

                    db.AllUserDatas.Remove(s);
                    db.SaveChanges();
                    result = true;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(result);
        }


        // CheckUsernameAvailability
        public JsonResult CheckUsernameAvailability(string userdata)
        {
            System.Threading.Thread.Sleep(200);
            var SeachData = db.AllUserDatas.Where(x => x.Email == userdata).FirstOrDefault();
            if (SeachData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }

        }

        // get all data 
      public JsonResult GetAllDataUser(string term)  // must the name is " term" because jquery 
        {
            List<string> lis = db.AllUserDatas.Where(x => x.F_Name.StartsWith(term)).Select(y => y.F_Name).Distinct().ToList();

            return Json(lis, JsonRequestBehavior.AllowGet);

        }

        // check email
        [NonAction]
        public bool IsEmailExist(string email)
        {

            var x = db.AllUserDatas.Where(a => a.Email == email).FirstOrDefault();

            return x != null;

        }

        // send the password to gmail
        [NonAction]
        public void SendPasswordToGmail(string YourEmail, string Name, string Password)
        {


            var FromEmail = new MailAddress("eslamturboo419@gmail.com", "turbooo");
            var ToEmail = new MailAddress(YourEmail);
            var FromEmailPass = "amramr987654321";



            string Subject = "from My Website    //// " + Name + "   /////   ";
            string Body = "<br/><br/><br/><br/> " + Password + " <br/><br/>"   + "  Here Is the URL " +
                "    <a href='http://localhost:50755/ ' class='btn btn-primary'> Click Here </a> ";


            var stmp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(FromEmail.Address, FromEmailPass)
            };

            using (var message = new MailMessage(FromEmail, ToEmail)
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            })

                try
                {
                    stmp.Send(message);
                }
                catch (Exception)
                {
                    throw;
                }


        }







    }
}