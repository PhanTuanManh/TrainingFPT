using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Helpers;
using TrainingFPT.Models;
using TrainingFPT.Models.Queries;

namespace TrainingFPT.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            //{
            //    return RedirectToAction(nameof(LoginController.Index), "Login");
            //}

            CategoryViewModel categoryViewModel = new CategoryViewModel();
            categoryViewModel.CategoryDetailList = new List<CategoryDetail>();
            var dataCategory = new CategoryQuery().GetAllCategories(SearchString, Status);
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
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
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


        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new CategoryQuery().DeleteItemCategory(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(CategoryController.Index), "Category");
        }

        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            CategoryDetail categoryDetail = new CategoryQuery().GetDataCategoryById(id);
            return View(categoryDetail);
        }

        [HttpPost]
        public IActionResult Edit(CategoryDetail categoryDetail, IFormFile PosterImage)
        {
            try
            {
                var detail = new CategoryQuery().GetDataCategoryById(categoryDetail.Id);
                string uniquePosterImage = detail.PosterNameImage; // lay lai ten anh cu truoc khi thay anh moi (neu co)
                // nguoi dung co muon thay anh poster category hay ko?
                if (categoryDetail.PosterImage != null)
                {
                    // co muon thay doi anh
                    uniquePosterImage = UploadFileHelper.UploadFile(PosterImage);
                }
                bool update = new CategoryQuery().UpdateCategoryById(
                    categoryDetail.Name,
                    categoryDetail.Description,
                    uniquePosterImage,
                    categoryDetail.Status,
                    categoryDetail.Id );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(CategoryController.Index), "Category");
            }
            catch (Exception ex)
            {
                // return Ok(ex.Message);
                return View(categoryDetail);
            }
        }
    }
}
