using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            model = new LoginQuery().CheckUserLogin(model.UserName, model.Password);
            if (string.IsNullOrEmpty(model.Id) || string.IsNullOrEmpty(model.Email))
            {
                // dang nhap tai khoan khong ton tai trong database
                // thong bao cho nguoi dung biet
                ViewData["MessageLogin"] = "Account invalid";
                return View(model);
            }

            // luu thong tin nguoi dung vao session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                HttpContext.Session.SetString("SessionUserId", model.Id);
                HttpContext.Session.SetString("SessionUsername", model.UserName);
                HttpContext.Session.SetString("SessionRoleId", model.RoleId);
                HttpContext.Session.SetString("SessionEmail", model.Email);
            }

            // chuyen vao trang home
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // xoa het session tao ra luc login
            // quay ve lai trang dang nhap
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserId")))
            {
                // xoa session
                HttpContext.Session.Remove("SessionUserId");
                HttpContext.Session.Remove("SessionUsername");
                HttpContext.Session.Remove("SessionRoleId");
                HttpContext.Session.Remove("SessionEmail");
            }
            // quay ve trang dang nhap
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
