using Microsoft.AspNetCore.Http;
using Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Model.DTO
{
    public class UserDetailsDto
    {
        public int UserId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "NameRequired", ErrorMessage = null)]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        private string? _userName { get; set; }
        public string UserName { get { return _userName ?? ""; } set => this._userName = FirstName + " " + LastName; }
        public string? Email { get; set; }
        public short UserType { get; set; }
        public string? PasswordHash { get; set; }
        public string? Password { get; set; }
        public string? ProfileImage { get; set; }
        public IFormFile? Image { get; set; }
        public string? ProfileImageUrl { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "PhoneNumberRequired", ErrorMessage = null)]
        [StringLength(15, MinimumLength = 7, ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "PhoneNumberLength", ErrorMessage = null)]
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? AccessToken { get; set; }
        public string? Token { get; set; }
        public string? ResetToken { get; set; }
        public string? EmailVerificationToken { get; set; }
    }
}
