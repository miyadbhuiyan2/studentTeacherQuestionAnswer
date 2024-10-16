using Microsoft.AspNetCore.Mvc;
using studentTeacherQuestionAnswer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace studentTeacherQuestionAnswer.Controllers
{
    public class HomeController : Controller
    {
        private readonly StqaContext context;

        public HomeController(StqaContext context)
        {
            this.context = context;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Answer(Answer ans)
        {
            if (ModelState.IsValid)
            {
                ans.Aby = HttpContext.Session.GetInt32("UserSession");
                await context.Answers.AddAsync(ans);
                await context.SaveChangesAsync();
                TempData["Success"] = "Answered Successfully";
                return RedirectToAction("Dashboard");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Reply(IFormCollection form)
        {

            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                
                     Reply rep = new Reply();
                     rep.Rby = HttpContext.Session.GetInt32("UserSession");
                     rep.Reply1 = form["Reply"];
                     rep.Rfor = Int32.Parse(form["ReplyFor"]);
                     await context.Replies.AddAsync(rep);
                     await context.SaveChangesAsync();
                     TempData["Success"] = "Replied Successfully";
                     return RedirectToAction("Dashboard");

            }
            else
            {
                return RedirectToAction("Login");
            }

        }

        [HttpPost]
        public IActionResult Login(UserInfo user)
        {
            var myUser = context.UserInfos.Where(x => x.EmailId == user.EmailId && x.Password == user.Password).FirstOrDefault();
            if (myUser != null) {
                HttpContext.Session.SetInt32("UserSession",myUser.UserId);
                HttpContext.Session.SetString("UserTypeSession", myUser.UserType);
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
            }
            return View();
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserInfo user)
        {
            if (ModelState.IsValid)
            {
                await context.UserInfos.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Success"] = "Registred Successfully";
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult ViewQuestion(int id)
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");

            }
            else
            {
                return RedirectToAction("Login");
            }
            ViewBag.id = id;
            var vQuestions = (
                                  from ui in context.UserInfos
                                  join qu in context.Questions on ui.UserId equals qu.Qby
                                  where(qu.Qid == id)
                                  select new
                                  {
                                      qID = qu.Qid,
                                      ques = qu.Question1,
                                      qDate = qu.Qdate,
                                      qBy = ui.Name,
                                      qUID = ui.UserId
                                  }).FirstOrDefault();
            ViewBag.vQ = vQuestions;

            var vAnswers = (
                                  from ui in context.UserInfos
                                  join ans in context.Answers on ui.UserId equals ans.Aby
                                  where (ans.Afor == id)
                                  select new
                                  {
                                      aID = ans.Aid,
                                      ans = ans.Answer1,
                                      aDate = ans.Adate,
                                      aBy = ui.Name,
                                      aUID = ui.UserId
                                  });
            ViewBag.vA = vAnswers;

            var vReplies = (
                                  from  ans in context.Answers 
                                  join rep in context.Replies on ans.Aid equals rep.Rfor
                                  join ui in context.UserInfos on rep.Rby equals ui.UserId
                                  where (ans.Afor == id)
                                  select new
                                  {
                                      rID = rep.Rid,
                                      rep = rep.Reply1,
                                      rDate = rep.Rdate,
                                      rFor = rep.Rfor,
                                      rBy = ui.Name,
                                      rUID = ui.UserId
                                  });
            ViewBag.vR = vReplies;

            return View();
        }
        public IActionResult DeleteQuestion(int id)
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");

                var delQuestion = (
                                 from ans in context.Answers
                                 join qu in context.Questions on ans.Afor equals qu.Qid
                                 where (qu.Qid == id)
                                 select new
                                 {
                                     aID = ans.Aid
                                 }).FirstOrDefault();

                if(delQuestion==null)
                {
                    context.Remove(context.Questions.Single(a => a.Qid == id));
                    context.SaveChanges();
                    TempData["Success"] = "Question Deleted Successfully";
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    TempData["Fail"] = "Question Delete Failed";
                    return RedirectToAction("Dashboard");
                }
                
            }
            else
            {
                return RedirectToAction("Login");
            }
            

        }
        public IActionResult MyQuestions()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");
                var myQuestions = (
                                 from ui in context.UserInfos
                                 join qu in context.Questions on ui.UserId equals qu.Qby
                                 where (qu.Qby == HttpContext.Session.GetInt32("UserSession"))
                                 select new
                                 {
                                     qID = qu.Qid,
                                     ques = qu.Question1,
                                     qDate = qu.Qdate,
                                     qBy = ui.Name,
                                     qUID = ui.UserId
                                 }).OrderByDescending(x => x.qDate);
                ViewBag.mQ = myQuestions;

            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        public IActionResult MyAnswers()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");

            }
            else
            {
                return RedirectToAction("Login");
            }

            var myAnsweredQuestions = (
                                  from ui in context.UserInfos
                                  join qu in context.Questions on ui.UserId equals qu.Qby
                                  join ans in context.Answers on qu.Qid equals ans.Afor
                                  join uii in context.UserInfos on ans.Aby equals uii.UserId
                                  where (ans.Aby == HttpContext.Session.GetInt32("UserSession"))
                                  select new
                                  {
                                      qID = qu.Qid,
                                      ques = qu.Question1,
                                      qDate = qu.Qdate,
                                      qBy = ui.Name,
                                      qUID = ui.UserId
                                  });
            ViewBag.mAQ = myAnsweredQuestions;

            var myAnswers = (
                                 from ans in context.Answers 
                                 join uii in context.UserInfos on ans.Aby equals uii.UserId
                                 where (ans.Aby == HttpContext.Session.GetInt32("UserSession"))
                                 select new
                                 {
                                     aID = ans.Aid,
                                     ans = ans.Answer1,
                                     aDate = ans.Adate,
                                     aFor = ans.Afor,
                                     aBy = uii.Name,
                                     aUID = uii.UserId
                                 });
            ViewBag.mA = myAnswers;

            var myReplies = (
                                 from rep in context.Replies
                                 join uii in context.UserInfos on rep.Rby equals uii.UserId
                                 where (rep.Rby == HttpContext.Session.GetInt32("UserSession"))
                                 select new
                                 {
                                     rID = rep.Rid,
                                     rep = rep.Reply1,
                                     rDate = rep.Rdate,
                                     rFor=rep.Rfor,
                                     rBy = uii.Name,
                                     rUID = uii.UserId
                                 });
            ViewBag.mR = myReplies;



            return View();
        }
        public IActionResult Question()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");

            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Question(Question q)
        {
            if (ModelState.IsValid)
            {
                q.Qby = HttpContext.Session.GetInt32("UserSession");
                await context.Questions.AddAsync(q);
                await context.SaveChangesAsync();
                TempData["Success"] = "Question Added Successfully";
                return RedirectToAction("Dashboard");
            }
            return View();
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetInt32("UserSession");
                ViewBag.MySessionType = HttpContext.Session.GetString("UserTypeSession");
                 var dQuestions = (
                                  from ui in context.UserInfos
                                  join qu in context.Questions on ui.UserId equals qu.Qby
                                  select new
                                  {
                                      qID =qu.Qid,
                                      ques=qu.Question1,
                                      qDate=qu.Qdate,
                                      qBy=ui.Name,
                                      qUID=ui.UserId
                                  }).OrderByDescending(x => x.qDate);
                ViewBag.dQ = dQuestions;

            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
