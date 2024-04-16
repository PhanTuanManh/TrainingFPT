using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Helpers;
using TrainingFPT.Models.Queries;
using TrainingFPT.Models;
using System.Net.NetworkInformation;

namespace TrainingFPT.Controllers
{
    public class TraineeController : Controller
    {
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserDetailList = new List<UserDetail>();
            var dataUser = new UserQuery().GetAllUsers(SearchString, Status, 4 );
            foreach (var item in dataUser)
            {
                userViewModel.UserDetailList.Add(new UserDetail
                {
                    Id = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,
                    Avatar = item.Avatar,
                    Phone = item.Phone,
                    Address = item.Address,
                    BirthDay = item.BirthDay,
                    Gender = item.Gender,
                    ExtraCode = item.ExtraCode,
                    Education = item.Education,
                    ProgramingLang = item.ProgramingLang,
                    Skills = item.Skills,
                    ToeicScore = item.ToeicScore,
                    IpClient = item.IpClient,
                    LastLogin = item.LastLogin,
                    LastLogout = item.LastLogout,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(userViewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.UserDetailList = new List<UserDetail>();
            var dataRole = new UserQuery().GetRole();
            UserDetail model = new UserDetail();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserDetail user, IFormFile AvatarFile)
        {
           
            if (ModelState.IsValid)
                {
                    // khong co loi tu phia nguoi dung
                    // upload file va lay dc ten file save database
                    string avatarNameFile = null;
                    if(AvatarFile != null)
                    {
                    var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
                    var fileExtension = Path.GetExtension(AvatarFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("AvatarFile", "Invalid file format. Only PNG, JPG, and JPEG formats are allowed.");
                        return View(user);
                    }
                    avatarNameFile = UploadFileHelper.UploadFile(AvatarFile);
                }
                   
                    try
                    {
                        int idInsetCate = new UserQuery().InsertItemUser(4, user.UserName, user.Password, user.Email, user.Phone, user.Address, user.BirthDay, user.Gender, user.ExtraCode, avatarNameFile, user.Education, user.ProgramingLang, user.ToeicScore, user.Skills, user.IpClient, user.Status);
                        if (idInsetCate > 0)
                        {
                            TempData["saveStatus"] = true;
                        }
                        else
                        {
                            TempData["saveStatus"] = false;
                        }
                    }
                    catch (Exception ex)
                    {

                        TempData["saveStatus"] = false;
                    return Ok(ex.Message);
                    }
                    return RedirectToAction(nameof(TraineeController.Index), "Trainee");
                }
            
            
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.UserDetailList = new List<UserDetail>();
                var dataRole = new UserQuery().GetRole();
                foreach (var item in dataRole)
                {
                    userViewModel.UserDetailList.Add(new UserDetail
                    {
                        RoleId = item.RoleId,
                        NameRole = item.NameRole

                    });
                }
                IEnumerable<UserDetail> userDetails = userViewModel.UserDetailList;
                ViewBag.userViewModel = userDetails;

                return View(user);
            
            
        }
        [HttpGet]
        public IActionResult ViewDetail(int id)
        {
            UserDetail userDetail = new UserQuery().GetViewDetail(id);
            
            return View(userDetail);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            UserDetail userDetail = new UserQuery().GetViewDetail(id);

            return View(userDetail);
        }
        [HttpPost]
        public IActionResult Edit(UserDetail userDetail, IFormFile AvatarFile)
        {
            try
            {
                var detail = new UserQuery().GetViewDetail(userDetail.Id);
                string avatar = detail.Avatar; // lay lai ten anh cu truoc khi thay anh moi (neu co)
                //nguoi dung co muon thay anh poster category hay ko?
                if (userDetail.AvatarFile != null)
                {
                    var allowedExtensions = new string[] { ".png", ".jpg", ".jpeg" };
                    var fileExtension = Path.GetExtension(AvatarFile.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("AvatarFile", "Invalid file format. Only PNG, JPG, and JPEG formats are allowed.");
                        return View(userDetail);
                    }
                }
                bool update = new UserQuery().UpdateUserById(
                     userDetail.UserName, userDetail.Password, userDetail.Email, userDetail.Phone, userDetail.Address, userDetail.BirthDay, userDetail.Gender, userDetail.ExtraCode, avatar, userDetail.Education, userDetail.ProgramingLang, userDetail.ToeicScore, userDetail.Skills, userDetail.IpClient, userDetail.Status, userDetail.Id);
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(TraineeController.Index), "Trainee");
            }
            catch (Exception ex)
            {

                return View(userDetail);
                //return Ok(uniquePosterImage);
            }

        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new UserQuery().DeleteItemUser(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(TraineeController.Index), "Trainee");
        }
    }
     
}
