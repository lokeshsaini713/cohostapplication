using Shared.Common.Enums;

namespace Business.Communication
{
    public interface INotificationService
    {
        Task EmailVerification(string toEmail, string name, string token, string subject,int devicetype);
        Task SendResetPasswordEmail(string emailsubject, string token, string toEmail, string? name);
        Task SendResetPasswordEmailToWebUser(string emailsubject, string token, string toEmail, string name);
        Task SendContactUsMailToAdmin(string emailsubject, string userName, string userEmail, string query);
    }
}
