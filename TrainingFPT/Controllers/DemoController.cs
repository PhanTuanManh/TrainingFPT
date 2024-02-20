using Microsoft.AspNetCore.Mvc;

namespace TrainingFPT.Controllers
{
    public class DemoController : Controller
    {
        public string Index()
        {
            // Demo/Index
            return "Hello word";
        }

        // Demo/Test
        public string Test()
        {
            return "ASP .Net core MVC";
        }

        // Demo/IT0503
        public IActionResult IT0503()
        {
            // tra ve 1 giao dien view
            return View();
        }
    }
}
