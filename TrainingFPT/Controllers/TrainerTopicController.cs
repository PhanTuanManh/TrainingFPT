using Microsoft.AspNetCore.Mvc;
using TrainingFPT.Models.Queries;
using TrainingFPT.Models;
using TrainingFPT.Helpers;
using System.Net.NetworkInformation;

namespace TrainingFPT.Controllers
{
    public class TrainerTopicController : Controller
    {
        public IActionResult Index(int SearchString)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUsername")))
            {
                return RedirectToAction(nameof(LoginController.Index), "Login");
            }
            TrainerTopicViewModel trainerTopicViewModel = new TrainerTopicViewModel();
            trainerTopicViewModel.TrainerTopicDetailList = new List<TrainerTopicDetail>();
            var dataUser = new TrainerTopicQuery().GetAllTrainerTopic();
            foreach (var item in dataUser)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                   UserName = item.UserName,
                   UserId = item.UserId,
                   TopicId = item.TopicId,
                   TopicName = item.TopicName,
                   CreatedAt = item.CreatedAt,
                });
            }
            return View(trainerTopicViewModel);
        }
    
        [HttpGet]
        public IActionResult Add()
        {
            
            TrainerTopicViewModel trainerTopicViewModel = new TrainerTopicViewModel();
            trainerTopicViewModel.TrainerTopicDetailList = new List<TrainerTopicDetail>();
            var dataUser = new TrainerTopicQuery().GetTrainer();
            foreach (var item in dataUser)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                     UserName= item.UserName,
                    UserId = item.Id

                });
            }
            var dataTopic = new TrainerTopicQuery().GetTopic();
            foreach (var item in dataTopic)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                    TopicName = item.Name,
                    TopicId = item.Id

                });
            }
            IEnumerable<TrainerTopicDetail>  trainerTopicDetails= trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.trainerTopicViewModel = trainerTopicDetails;
            IEnumerable<TrainerTopicDetail> topic = trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.topic = topic;
            TrainerTopicDetail model = new TrainerTopicDetail();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(TrainerTopicDetail trainerTopicDetail)
        {
            if (ModelState.IsValid)
            {
                // khong co loi tu phia nguoi dung
                // upload file va lay dc ten file save database
              
                try
                {
                    int idInsetCate = new TrainerTopicQuery().InsertItemTrainerTopic(trainerTopicDetail.UserId, trainerTopicDetail.TopicId);
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
                    //return Ok(ex.Message);
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(TrainerTopicController.Index), "TrainerTopic");
            }
            TrainerTopicViewModel trainerTopicViewModel = new TrainerTopicViewModel();
            trainerTopicViewModel.TrainerTopicDetailList = new List<TrainerTopicDetail>();
            var dataUser = new TrainerTopicQuery().GetTrainer();
            foreach (var item in dataUser)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                    UserName = item.UserName,
                    UserId = item.Id

                });
            }
            var dataTopic = new TrainerTopicQuery().GetTopic();
            foreach (var item in dataTopic)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                    TopicName = item.Name,
                    TopicId = item.Id

                });
            }
            IEnumerable<TrainerTopicDetail> trainerTopicDetails = trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.trainerTopicViewModel = trainerTopicDetails;
            IEnumerable<TrainerTopicDetail> topic = trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.topic = topic;

            return View(trainerTopicDetail );
        }
        [HttpGet]
        public IActionResult Edit(int userId = 0, int topicId = 0)
        {
            TrainerTopicViewModel trainerTopicViewModel = new TrainerTopicViewModel();
            trainerTopicViewModel.TrainerTopicDetailList = new List<TrainerTopicDetail>();
            var dataUser = new TrainerTopicQuery().GetTrainer();
            foreach (var item in dataUser)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                    UserName = item.UserName,
                    UserId = item.Id

                });
            }
            var dataTopic = new TrainerTopicQuery().GetTopic();
            foreach (var item in dataTopic)
            {
                trainerTopicViewModel.TrainerTopicDetailList.Add(new TrainerTopicDetail
                {
                    TopicName = item.Name,
                    TopicId = item.Id

                });
            }
            IEnumerable<TrainerTopicDetail> trainerTopicDetails = trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.trainerTopicViewModel = trainerTopicDetails;
            IEnumerable<TrainerTopicDetail> topic = trainerTopicViewModel.TrainerTopicDetailList;
            ViewBag.topic = topic;

            TrainerTopicDetail trainerTopicDetail = new TrainerTopicQuery().GetDataTrainerTopicById(userId, topicId);
            return View(trainerTopicDetail);
        }
        [HttpPost]
        public IActionResult Edit(TrainerTopicDetail trainerTopicDetail)
        {
            try
            {
                var detail = new TrainerTopicQuery().GetDataTrainerTopicById(trainerTopicDetail.UserId);
             
                bool update = new TrainerTopicQuery().UpdateTrainerTopicById(
                    trainerTopicDetail.UserId,
                    trainerTopicDetail.TopicId
                    );
                if (update)
                {
                    TempData["updateStatus"] = true;
                }
                else
                {
                    TempData["updateStatus"] = false;
                }
                return RedirectToAction(nameof(TrainerTopicController.Index), "TrainerTopic");
            }
            catch (Exception ex)
            {
                //return Ok(ex.Message);
                return View(trainerTopicDetail);
                //return Ok(uniquePosterImage);
            }
        }
       
    }
}
