using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrgProject.Models;

namespace OrgProject.Controllers
{
     [AllowAnonymous]
    public class QuizWithYearController : Controller
    {
        private MyDBEntities db = new MyDBEntities();

 

        public ActionResult QuizBeforeStart(int id)
        {
          
            List<Question> questions = db.Questions.Where(x => x.Course_id == id).OrderBy(x => x.Id).ToList();

                    Queue<Question> queu = new Queue<Question>();

                    foreach (Question a in questions)
                    {
                        queu.Enqueue(a);
                    }

                    TempData["questions"] = queu;
                    TempData["score"] = 0;
                    TempData.Keep();
                    return RedirectToAction("QuizStart");
             

        }

        public ActionResult QuizStart()
        {
            Question q = null;
            if (TempData["questions"] != null)
            {
                Queue<Question> qlist = (Queue<Question>)TempData["questions"];
                int length = qlist.Count;
                if (qlist.Count > 0)
                {
                    q = qlist.Peek();
                    qlist.Dequeue();
                    TempData["questions"] = qlist;
                    TempData.Keep();

                    length = length - 1;
                }
                else
                {
                    return RedirectToAction("EndExam");
                }
                
                ViewBag.numberthis = length;
            }
            else
            {
                return RedirectToAction("StudentExam");
            }

            return View(q);
        }

        [HttpPost]
        public ActionResult QuizStart(Question q )
        {
            var answer = db.Questions.Where(x => x.Id == q.Id).FirstOrDefault().Answer_Correct.ToLower();

            string correct = null;

            if (q.Answer_1 != null)
            {
                correct = "a";
            }
            else if (q.Answer_2 != null)
            {
                 
                correct = "b";
            }
            else if (q.Answer_3 != null)
            {
               
                correct = "c";

            }
            else if (q.Answer_4 != null)
            {
                correct = "d";
            }
            
            //if (correct.Equals(q.Answer_Correct.ToLower()))
            if (correct.Equals(answer) )
            {
                TempData["score"] = Convert.ToInt32(TempData["score"]) + 1;
            }

            TempData.Keep();
            return RedirectToAction("QuizStart");
        }


        public ActionResult EndExam()
        {
            return View();
        }




    }
}