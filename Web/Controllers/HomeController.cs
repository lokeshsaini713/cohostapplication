using Business.IServices;
using Data;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using Shared.Model.Notification;
using Shared.Model.Request.WebUser;
using Shared.Utility;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController(AppDbContext context, IAccountService accountService, ILogger<HomeController> logger, IWebHostEnvironment env, EmailService emailService) : Controller
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
                .Take(6)
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
        public async Task<IActionResult> BookConsultation([FromForm] ConsultationViewModel lead)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var leadEntity = new Lead
            {
                FullName = lead.FullName,
                Email = lead.Email,
                Phone = lead.PhoneNumber,
                Company = "home page request",
                Message = lead.Message,
                NDA = true,
                Source = "Homepage",
                PageUrl ="index"
            };
            try
            {
                context.Leads.Add(leadEntity);
                context.SaveChanges();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            // Send Email
            //var res = await accountService.SaveContactUsDetails(new ContactUsRequestModel { Email= lead.Email ,Name= lead.FullName,Query= lead.Message});

            // 🔔 ADMIN EMAIL
            var adminBody = $@"
            <h2>New Consultation Request</h2>
            <p><strong>Name:</strong> {lead.FullName}</p>
            <p><strong>Email:</strong> {lead.Email}</p>
            <p><strong>Phone:</strong> {lead.PhoneNumber}</p>
            <p><strong>Company:</strong> N/A(homepage)</p>
            <p><strong>NDA:</strong> Yes</p>
            <p><strong>Message:</strong><br/>{lead.Message}</p>
        ";

            emailService.Send(
                to: "info@cohostweb.com",
                subject: "New Free Consultation Lead",
                body: adminBody
            );

            // ✅ THANK YOU EMAIL TO USER
            SendThankYouEmail(new LeadRequest { Email=lead.Email,FullName=lead.FullName,Message=lead.Message,Phone=lead.PhoneNumber, Company=lead.PhoneNumber});

            return Json(new
            {
                success = true,
                dis = "For reaching out! Our team will contact you shortly."
            });
        }

        private void SendThankYouEmail(LeadRequest lead)
        {
            var body = $@"
            <p>Hi {lead.FullName},</p>

            <p>Thank you for reaching out to <strong>Cohost Web</strong>.</p>

            <p>We’ve received your request for a <strong>free 30-minute strategy consultation</strong>.
            One of our senior consultants will contact you within <strong>24 hours</strong>.</p>

            <p><strong>What happens next?</strong></p>
            <ul>
              <li>We review your requirements</li>
              <li>Prepare technical insights & recommendations</li>
              <li>Discuss timelines, architecture & cost estimates</li>
            </ul>

            <p>If you’d like to talk sooner, you can:</p>
            <ul>
              <li>📞 Call us: +91 9024255861</li>
              <li>💬 WhatsApp us anytime</li>
              <li>📅 Schedule a call via our website</li>
            </ul>

            <p>Best regards,<br/>
            <strong>Cohost Web Team</strong><br/>
            https://www.cohostweb.com</p>
        ";

            emailService.Send(
                to: lead.Email,
                subject: "We’ve Received Your Consultation Request",
                body: body
            );
        }
        [Route("contact-us")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}