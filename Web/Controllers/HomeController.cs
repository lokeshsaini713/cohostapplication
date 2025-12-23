using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Shared.Model.Notification;
using Shared.Utility;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("PrivacyPolicy")]
        public IActionResult Privacy()
        {
            return View();
        }


        [Route("TermsAndConditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// test notification send functionality
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> NotificationSend()
        {
            var pushNotificationRequestModel = new PushNotificationRequestModel
            {
                DeviceToken = "fXCvtBrPQK6ObiKkJGGji1:APA91bG_6VXyrY78-gMem8jngEnzx_sUTXRM5ZfI9vF0lAYl-S7IZyfv9lf2ICZShw8w_wmi48npNYlKxsW9aUFaMCcGgbVExvXP4xBbf4smCqs67ajFVudVCMTqgNsI_ONvhPsyRadY",
                Title = "New Approach",
                Message = "Message",
                MsgId = 1,
                NotificationType = "",
                NotificationId = 1,
                ClickAction = "",
                Data = new PushNotificationDataModel
                {
                    UserName = "Ujjwal",
                    UserImage = "https://google.com"
                }
            };

            _ = await PushNotifications.SendPushNotification(pushNotificationRequestModel);

            return View();
        }
    }
}