namespace Shared.Model.Request.Account
{
    public class UpdateDeviceTokenRequest
    {
        public string? Email { get; set; }
        public string? DeviceToken { get; set; }
        public short DeviceType { get; set; }
        public string? AccessToken { get; set; }
    }
}
