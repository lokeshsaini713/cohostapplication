using Shared.Common.Enums;
using Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Model.Request.Account
{
    public class RegistrationRequest
    {
        [EmailAddress(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "EmailValid", ErrorMessage = null)]
        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "EmailRequired", ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "EmailLength", ErrorMessage = null)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "FirstNameRequired", ErrorMessage = null)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "LastNameRequired", ErrorMessage = null)]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "PasswordRequired", ErrorMessage = null)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#\s]{6,20}$", ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "PasswordValid", ErrorMessage = null)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "ConfirmPasswordRequired", ErrorMessage = null)]
        [Compare("Password", ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "ConfirmPasswordValid", ErrorMessage = null)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public bool TermsAndCondtion { get; set; }
        public UserTypes UserType { get; set; }
        public DeviceTypeEnum DeviceType { get; set; } 
    }
}
