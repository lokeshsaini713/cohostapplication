namespace Shared.Common
{
    public class SiteKeys
    {
        public static int UtcOffsetInSecond { get; set; }
        public static string? DeviceToken { get; set; }
        public static string? SitePhysicalPath { get; set; }
        public static string? SiteUrl { get; set; }
        public static string? UtcOffset { get; set; }
        public static int UtcOffsetInSecond_API { get; set; }
        public static string? AccessToken { get; set; }
        public static string? FCMServerKey { get; set; }
        public static string? FCMSenderId { get; set; }
        public static string? EncryptDecryptKey { get; set; }


        #region Application Statics 

        #endregion
    }

    public static class Constants 
    {
        public const int DefultPageNumber = 1;
        public const int DefultPageSize = 10;
        public const string DefaultUserImage = "assets/images/DefaultImage.png";
        public const string UserImageFolderPath = "Uploads/UserImages/";
        public const string DefaultUserPng = "DefaultImage.png";
        public const string EmailTempaltePath = "wwwroot\\EmailTemplate";
        public const string AppName = "Template";
    }
}
