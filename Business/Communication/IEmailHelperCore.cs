namespace Business.Communication
{
    public interface IEmailHelperCore
    {
        Task<bool> Send(string body, string recipient, string subject, string recipientCC, string recipientBCC);
        string GenerateEmailTemplateFor(string templateName, params MessageKeyValue[] args);
        string GenerateEmailTemplateWithfull(string filePath, params MessageKeyValue[] args);
        bool IsValidEmailAddress(string emailAddress);
    }
}
