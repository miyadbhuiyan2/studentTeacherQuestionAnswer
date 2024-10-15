using Microsoft.AspNetCore.Mvc;
using studentTeacherQuestionAnswer.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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

            ViewBag.id = id;
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
           
            var myAnswers = (
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
                                      qUID = ui.UserId,

                                      aID = ans.Aid,
                                      ans = ans.Answer1,
                                      aDate = ans.Adate,
                                      aFor = ans.Afor,
                                      aBy = uii.Name,
                                      aUID = uii.UserId
                                  });
            ViewBag.mA = myAnswers;
            ViewBag.mAA = myAnswers;

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
