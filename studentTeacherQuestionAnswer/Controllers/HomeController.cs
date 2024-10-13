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
        public IActionResult Login(UserInfo user)
        {
            var myUser = context.UserInfos.Where(x => x.EmailId == user.EmailId && x.Password == user.Password).FirstOrDefault();
            if (myUser != null) {
                HttpContext.Session.SetString("UserSession",myUser.EmailId);
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

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession");
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
