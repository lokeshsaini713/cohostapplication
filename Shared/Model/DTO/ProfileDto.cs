namespace Shared.Model.DTO
{
    public class ProfileDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? ProfileImage { get; set; }
        public string? AuthorizationToken { get; set; }
        public string? AccessToken { get; set; }
        public short UserType { get; set; }
    }
}
