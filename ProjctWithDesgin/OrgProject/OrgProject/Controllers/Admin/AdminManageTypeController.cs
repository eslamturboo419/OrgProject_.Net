using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminManageTypeController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

        // GET: Types
        public  ActionResult Index()
        {
            return View(  db.Types.OrderByDescending(x => x.id).ToList());
        }
       

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult>  Create([Bind(Include = "id,Type1")]   Models.Type type)
        {
            if (ModelState.IsValid)
            {
                db.Types.Add(type);
               await db.SaveChangesAsync();
                TempData["sucessMsg"] = " Saved OK";
                return RedirectToAction("Index");
            }

            return View(type);
        }



    }
}