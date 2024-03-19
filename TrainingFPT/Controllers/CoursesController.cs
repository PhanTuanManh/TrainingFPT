using Microsoft.AspNetCore.Mvc;

namespace TrainingFPT.Controllers
{
    public class CoursesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
    }
}
