using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            //{
            //    return RedirectToAction(nameof(LoginController.Index), "Login");
            //}

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CategoryDetailList = new List<CategoryDetail>();
            var dataCategory = new CategoryQuery().GetAllCategories();
            foreach (var item in dataCategory)
            {
                categoryViewModel.CategoryDetailList.Add(new CategoryDetail
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    PosterNameImage = item.PosterNameImage,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            return View(categoryViewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            CategoryDetail model = new CategoryDetail();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CategoryDetail category, IFormFile PosterImage)
        {
            if (ModelState.IsValid)
            {
                // khong co loi tu phia nguoi dung
                // upload file va lay dc ten file save database
                string filePosterImage = UploadFileHelper.UploadFile(PosterImage);
                try
                {
                    int idInsetCate = new CategoryQuery().InsertItemCategory(category.Name, category.Description, filePosterImage, category.Status);
                    if (idInsetCate > 0)
                    {
                        TempData["saveStatus"] = true;
                    }
                    else
                    {
                        TempData["saveStatus"] = false;
                    }
                }
                catch
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");
            }
            return View(category);
        }
    }
}
