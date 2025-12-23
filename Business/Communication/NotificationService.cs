using Microsoft.Extensions.Options;
using Shared.Common;
using Shared.Common.Enums;
using Shared.Resources;

namespace Business.Communication
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailFunctions _emailFunctions;

        public NotificationService(IEmailFunctions emailFunctions)
        {
            _emailFunctions = emailFunctions;
        }

        public async Task EmailVerification(string toEmail, string name, string token, string subject, int devicetype)
        {
            var emailConfirmationLink = $"{SiteKeys.SiteUrl}Account/EmailVerification?token={System.Web.HttpUtility.UrlEncode(token)}&type={devicetype}";
            await _emailFunctions.EmailVerification(toEmail, subject, name, emailConfirmationLink);
        }

        public async Task SendResetPasswordEmail(string emailsubject, string token, string toEmail, string? name)
        {
            var reseturl = $"{SiteKeys.SiteUrl}Account/ResetPassword?token={token}";
            await _emailFunctions.SendResetPasswordEmail(ResourceString.ForgetPasswordSubject, reseturl, toEmail, name);
        }

        public async Task SendResetPasswordEmailToWebUser(string emailsubject, string token, string toEmail, string name)
        {
            var passwordResetLink = SiteKeys.SiteUrl + "Account/ResetPassword?Token=" + token;
            await _emailFunctions.SendResetPasswordEmailToWebUser(ResourceString.ForgetPasswordSubject, passwordResetLink, toEmail, name);
        }

        public async Task SendContactUsMailToAdmin(string emailsubject, string userName, string userEmail, string query)
        {
            await _emailFunctions.SendContactUsMailToAdmin(emailsubject, userName, userEmail, query);
        }
    }
}
