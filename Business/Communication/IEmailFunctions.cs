namespace Business.Communication
{
    public interface IEmailFunctions
    {
         Task EmailVerification(string toEmail, string emailsubject, string name, string emailVerificationLink);
         Task SendResetPasswordEmail(string emailsubject, string resetUrl, string toEmail, string? name);
         Task SendResetPasswordEmailToWebUser(string emailsubject, string resetUrl, string toEmail, string name);
         Task SendContactUsMailToAdmin(string emailsubject, string userName, string userEmail, string query);
    }
}
