using Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace Shared.Model.Request.WebUser
{
    public class ContactUsRequestModel
    {
        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "NameRequired", ErrorMessage = null)]
        [StringLength(100, MinimumLength = 3, ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "NameLength", ErrorMessage = null)]
        public string Name { get; set; } = string.Empty;


        [EmailAddress]
        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "EmailRequired", ErrorMessage = null)]
        [StringLength(100, ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "EmailLength", ErrorMessage = null)]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "QueryRequired", ErrorMessage = null)]
        [StringLength(500, MinimumLength = 15, ErrorMessageResourceType = typeof(ResourceString), ErrorMessageResourceName = "QueryLength", ErrorMessage = null)]
        public string Query { get; set; } = string.Empty;
    }
}
