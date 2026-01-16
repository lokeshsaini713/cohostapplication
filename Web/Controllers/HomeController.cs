using Business.IServices;
using Data;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Model.Notification;
using Shared.Model.Request.WebUser;
using Shared.Utility;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController(AppDbContext context, IAccountService accountService, ILogger<HomeController> logger, IWebHostEnvironment env) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("privacy-policy")]
        public IActionResult Privacy()
        {
            return View();
        }


        [Route("terms-conditions")]
        public IActionResult TermsAndConditions()
        {
            return View();
        }
        [Route("terms-of-use")]
        public IActionResult Termsofuse()
        {
            return View();
        }

        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("web-development")]
        public IActionResult Webdevelopment()
        {
            return View();
        }
        [Route("software-development")]
        public IActionResult Softwaredevelopment()
        {
            return View();
        }
        [Route("search-engine-optimization")]
        public IActionResult Seo()
        {
            return View();
        }
        [Route("app-development")]
        public IActionResult MobileAppDevelopment()
        {
            return View();
        }

        [Route("ecommerce-development")]
        public IActionResult EcommerceDevelopment()
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


        [HttpGet("latest")]
        public IActionResult Latest()
        {
            var data = context.Articles
                .Where(x => x.IsActive)              // ✅ FILTER
                .OrderBy(x => x.SortOrder)           // ✅ SORT
                .ThenByDescending(x => x.PublishedDate)
                .Take(3)
                .Select(x => new
                {
                    x.Title,
                    x.Slug,
                    x.ShortDescription,
                    x.ImagePath,
                    Date = x.PublishedDate.ToString("MMMM dd, yyyy"),
                    x.Category
                })
                .ToList();

            return Ok(data);
        }


        //        [HttpPost]
        //        [ValidateAntiForgeryToken]
        //        public IActionResult BookConsultation(ConsultationViewModel model)
        //        {
        //            if (!ModelState.IsValid)
        //            {
        //                return BadRequest(ModelState);
        //            }

        //            try
        //            {
        //                var mail = new MailMessage();
        //                mail.From = new MailAddress("your-email@gmail.com");
        //                mail.To.Add("lokeshsaini713@gmail.com");
        //                mail.Subject = "New Free Consultation Request";
        //                mail.Body = $@"
        //New Consultation Request:

        //Full Name: {model.FullName}
        //Email: {model.Email}
        //Phone: {model.PhoneNumber}

        //Message:
        //{model.Message}
        //";
        //                mail.IsBodyHtml = false;

        //                var smtp = new SmtpClient("smtp.gmail.com", 587)
        //                {
        //                    Credentials = new NetworkCredential("your-email@gmail.com", "YOUR_APP_PASSWORD"),
        //                    EnableSsl = true
        //                };

        //                smtp.Send(mail);

        //                return Ok(new { message = "Consultation request sent successfully" });
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode(500, "Error sending email");
        //            }
        //        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookConsultation([FromForm] ConsultationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //// Save to DB
            //var entity = new Consultation
            //{
            //    FullName = model.FullName,
            //    Email = model.Email,
            //    PhoneNumber = model.PhoneNumber,
            //    Message = model.Message
            //};

            //_context.Consultations.Add(entity);
            //_context.SaveChanges();

            // Send Email
            var res = await accountService.SaveContactUsDetails(new ContactUsRequestModel { Email= model.Email ,Name=model.FullName,Query=model.Message});

                        var mail = new MailMessage
                        {
                            From = new MailAddress("info@cohostweb.com"),
                            Subject = "New Consultation Request",
                            Body = $@"
            Name: {model.FullName}
            Email: {model.Email}
            Phone: {model.PhoneNumber}

            Message:
            {model.Message}
            "
                        };

                        mail.To.Add("lokeshsaini713@gmail.com");
                        System.Net.ServicePointManager.SecurityProtocol =
               SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
            //SmtpClient smtp = new SmtpClient();
            //smtp.Host = "mail.cohostweb.com";
            //smtp.UseDefaultCredentials = false;
            //smtp.EnableSsl = true;
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();
            //credentials.UserName = "info@cohostweb.com";
            //credentials.Password = "q8tuF32^8";
            //smtp.Credentials = credentials;
            //smtp.Port = 587;

            //            smtp.Timeout = 30000;


            //var smtp = new SmtpClient("target2earn.com", 587)
            //{
            //    Credentials = new NetworkCredential("target@target2earn.com", "target@12345"),
            //    EnableSsl = true
            //};
            string rootPath = env.ContentRootPath;   // physical path where app is running
                                                      // or if your template is in the web root (wwwroot), use:
                                                      // string rootPath = _env.WebRootPath;

            // combine with your relative folder
            string templateDirectory = Path.Combine(rootPath, "EmailTemplates");

            //smtp.Send(mail);

            return Json(new { success = res,dis= templateDirectory });
        }
        [Route("contact-us")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}