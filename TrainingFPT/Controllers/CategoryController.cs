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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }

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
            if (PosterImage != null && PosterImage.Length > 0)
            {
                // Kiểm tra loại tệp tin của PosterImage
                var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(PosterImage.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("PosterImage", "Invalid file format. Only PNG, JPG, and JPEG formats are allowed.");
                    // Trả về View với lỗi
                    return View(category);
                }

                // Kiểm tra kích thước của PosterImage
                var maxSize = 5 * 1024 * 1024; // 5MB
                if (PosterImage.Length > maxSize)
                {
                    ModelState.AddModelError("PosterImage", $"Maximum allowed file size is {maxSize} bytes.");
                    // Trả về View với lỗi
                    return View(category);
                }
            }

            // ModelState.IsValid sẽ kiểm tra các điều kiện đối với các thuộc tính khác của category
            if (ModelState.IsValid)
            {
                // Không có lỗi từ phía người dùng
                // Upload file và lấy tên file để lưu vào cơ sở dữ liệu
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

            // Nếu có lỗi ModelState.IsValid, trả về View với dữ liệu của category
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
                string uniquePosterImage = detail.PosterNameImage; // Lấy lại tên ảnh cũ trước khi thay ảnh mới (nếu có)

                // Người dùng có muốn thay ảnh đại diện hay không?
                if (PosterImage != null && PosterImage.Length > 0)
                {
                    // Kiểm tra loại tệp tin của PosterImage
                    var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
                    var fileExtension = Path.GetExtension(PosterImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("PosterImage", "Invalid file format. Only PNG, JPG, and JPEG formats are allowed.");
                        // Trả về View với lỗi
                        return View(categoryDetail);
                    }

                    // Kiểm tra kích thước của PosterImage
                    var maxSize = 5 * 1024 * 1024; // 5MB
                    if (PosterImage.Length > maxSize)
                    {
                        ModelState.AddModelError("PosterImage", $"Maximum allowed file size is {maxSize} bytes.");
                        // Trả về View với lỗi
                        return View(categoryDetail);
                    }

                    // Người dùng muốn thay đổi ảnh, thực hiện upload ảnh mới
                    uniquePosterImage = UploadFileHelper.UploadFile(PosterImage);
                }

                // Tiến hành cập nhật thông tin danh mục
                bool update = new CategoryQuery().UpdateCategoryById(
                    categoryDetail.Name,
                    categoryDetail.Description,
                    uniquePosterImage,
                    categoryDetail.Status,
                    categoryDetail.Id);

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
                // Xử lý ngoại lệ ở đây
                return View(categoryDetail);
            }
        }

    }
}
