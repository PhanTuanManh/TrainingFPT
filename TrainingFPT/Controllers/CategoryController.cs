using Microsoft.AspNetCore.Mvc;

namespace TrainingFPT.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
