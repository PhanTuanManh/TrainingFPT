using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Models.Queries;
using TrainingFPT.Models;
using TrainingFPT.Helpers;
using System.Net.NetworkInformation;
using TrainingFPT.Migrations;

namespace TrainingFPT.Controllers
{
    public class TopicsController : Controller
    {
        [HttpGet]
        public IActionResult Index(string SearchString, string Status)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            TopicViewModel topicViewModel = new TopicViewModel();
            topicViewModel.TopicDetailList = new List<TopicDetail>();
            var dataCategory = new TopicQuery().GetAllTopics(SearchString, Status);
            foreach (var item in dataCategory)
            {
                topicViewModel.TopicDetailList.Add(new TopicDetail
                {
                    Id = item.Id,
                    Name = item.Name,
                    NameCourse = item.NameCourse,
                    Description = item.Description,
                    NameVideo = item.NameVideo,
                    NameAudio = item.NameAudio,
                    DocumentNameTopic = item.DocumentNameTopic,
                    Status = item.Status,
                    Like = item.Like,
                    Star = item.Star,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt
                });
            }
            ViewData["keyword"] = SearchString;
            ViewBag.Status = Status;
            return View(topicViewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            TopicViewModel topicViewModel = new TopicViewModel();
            topicViewModel.TopicDetailList = new List<TopicDetail>();
            var dataCourse = new TopicQuery().GetCourse();
            foreach (var item in dataCourse)
            {
                topicViewModel.TopicDetailList.Add(new TopicDetail
                {
                    CourseId = item.CourseId,
                    NameCourse = item.NameCourse

                });
            }
            IEnumerable<TopicDetail> topicDetails = topicViewModel.TopicDetailList;
            ViewBag.topicViewModel = topicDetails;
            TopicDetail model = new TopicDetail();
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TopicDetail topic, IFormFile VideoFile, IFormFile AudioFile, IFormFile DocumentFile)
        {
            string fileAudio = "Null";
            string fileVideo = "Null";
            string fileDocumentTopic = "Null";
            if (VideoFile == null && AudioFile == null && DocumentFile == null)
            {
                TopicViewModel topicViewModel = new TopicViewModel();
                topicViewModel.TopicDetailList = new List<TopicDetail>();
                var dataCourse = new TopicQuery().GetCourse();
                foreach (var item in dataCourse)
                {
                    topicViewModel.TopicDetailList.Add(new TopicDetail
                    {
                        CourseId = item.CourseId,
                        NameCourse = item.NameCourse

                    });
                }
                IEnumerable<TopicDetail> topicDetails = topicViewModel.TopicDetailList;
                ViewBag.topicViewModel = topicDetails;

                return View(topic);
            }
            if (VideoFile != null)
            {
                var allowedExtensions = new string[] { ".mp4", ".avi", ".mkv" };
                var fileExtension = Path.GetExtension(VideoFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("VideoFile", "Invalid file format. Only MP4, AVI, and MKV formats are allowed.");
                    // Trả về View với lỗi
                    var dataCourse = new TopicQuery().GetCourse();
                    ViewBag.topicViewModel = dataCourse;
                    return View(topic);
                }

                fileVideo = UploadFileHelper.UploadFile(VideoFile);
            }
            else
            {
                ModelState.Remove("VideoFile");
            }
            if (AudioFile == null)
            {
                ModelState.Remove("AudioFile");
            }
            else
            {
                var allowedExtensions = new string[] { ".mp3", ".wav" };
                var fileExtension = Path.GetExtension(AudioFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("AudioFile", "Invalid file format. Only MP3 and WAV formats are allowed.");
                    // Trả về View với lỗi
                    var dataCourse = new TopicQuery().GetCourse();
                    ViewBag.topicViewModel = dataCourse;
                    return View(topic);
                }

                fileAudio = UploadFileHelper.UploadFile(AudioFile);
            }
            if (DocumentFile == null)
            {
                ModelState.Remove("DocumentFile");
            }
            else
            {
                var allowedExtensions = new string[] { ".doc", ".docx", ".pdf" };
                var fileExtension = Path.GetExtension(DocumentFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("DocumentFile", "Invalid file format. Only DOC, DOCX, and PDF formats are allowed.");
                    // Trả về View với lỗi
                    var dataCourse = new TopicQuery().GetCourse();
                    ViewBag.topicViewModel = dataCourse;
                    return View(topic);
                }

                fileDocumentTopic = UploadFileHelper.UploadFile(DocumentFile);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    int idInsetCate = new TopicQuery().InsertItemTopic(topic.Name, topic.CourseId, topic.Description, fileVideo, fileAudio, fileDocumentTopic, topic.Like, topic.Star, topic.Status);
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
                }
                return RedirectToAction(nameof(TopicsController.Index), "Topics");
            }
            if (VideoFile == null && AudioFile == null && DocumentFile == null)
            {
                TopicViewModel topicViewModel = new TopicViewModel();
                topicViewModel.TopicDetailList = new List<TopicDetail>();
                var dataCourse = new TopicQuery().GetCourse();
                foreach (var item in dataCourse)
                {
                    topicViewModel.TopicDetailList.Add(new TopicDetail
                    {
                        CourseId = item.CourseId,
                        NameCourse = item.NameCourse

                    });
                }
                IEnumerable<TopicDetail> topicDetails = topicViewModel.TopicDetailList;
                ViewBag.topicViewModel = topicDetails;
                return View(topic);
            }

            return View(topic);
        }

        [HttpGet]
        public IActionResult Edit(int id = 0)
        {
            TopicViewModel topicViewModel = new TopicViewModel();
            topicViewModel.TopicDetailList = new List<TopicDetail>();
            var dataCourse = new TopicQuery().GetCourse();
            foreach (var item in dataCourse)
            {
                topicViewModel.TopicDetailList.Add(new TopicDetail
                {
                    CourseId = item.CourseId,
                    NameCourse = item.NameCourse

                });
            }
            IEnumerable<TopicDetail> topicDetails = topicViewModel.TopicDetailList;
            ViewBag.topicViewModel = topicDetails;
            TopicDetail topicDetail = new TopicQuery().GetDataTopicById(id);
            return View(topicDetail);
        }
        [HttpPost]
        public IActionResult Edit(TopicDetail topicDetail, IFormFile VideoFile, IFormFile AudioFile, IFormFile DocumentFile)
        {
            try
            {
                var detail = new TopicQuery().GetDataTopicById(topicDetail.Id);

                string uniqueNameImage = detail.NameVideo;
                string uniqueAudioVideo = detail.NameAudio;
                string uniqueDocumentTopic = detail.DocumentNameTopic;
                if (topicDetail.VideoFile != null)
                {
                    // co muon thay doi anh
                    uniqueNameImage = UploadFileHelper.UploadFile(VideoFile);
                }
                if (topicDetail.AudioFile != null)
                {
                    // co muon thay doi anh
                    uniqueAudioVideo = UploadFileHelper.UploadFile(AudioFile);
                }
                if (topicDetail.DocumentFile != null)
                {
                    // co muon thay doi anh
                    uniqueDocumentTopic = UploadFileHelper.UploadFile(DocumentFile);
                }
                bool update = new TopicQuery().UpdateTopicById(
                topicDetail.Name,
                topicDetail.CourseId,
                topicDetail.Description, 
                uniqueNameImage, 
                uniqueAudioVideo, 
                uniqueDocumentTopic, 
                topicDetail.Like, 
                topicDetail.Star, 
                topicDetail.Status,
                topicDetail.Id
                );

                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }

                return RedirectToAction(nameof(CategoryController.Index), "Topics");
            }
            catch (Exception ex)
            {
                TopicViewModel topicViewModel = new TopicViewModel();
                topicViewModel.TopicDetailList = new List<TopicDetail>();
                var dataCourse = new TopicQuery().GetCourse();
                foreach (var item in dataCourse)
                {
                    topicViewModel.TopicDetailList.Add(new TopicDetail
                    {
                        CourseId = item.CourseId,
                        NameCourse = item.NameCourse

                    });
                }
                IEnumerable<TopicDetail> topicDetails = topicViewModel.TopicDetailList;
                ViewBag.topicViewModel = topicDetails;
                return View(topicDetail);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            bool del = new TopicQuery().DeleteItemTopic(id);
            if (del)
            {
                TempData["statusDel"] = true;
            }
            else
            {
                TempData["statusDel"] = false;
            }
            return RedirectToAction(nameof(TopicsController.Index), "Topics");
        }
    }
}
