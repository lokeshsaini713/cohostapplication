using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    [Table("UserDetail")]
    public class UserDetail
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public byte UserType { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ProfileImage { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsEmailVerified { get; set; }

        public string? EmailVerifiedToken { get; set; }

        public string? ResetPasswordToken { get; set; }

        public string? AccessToken { get; set; }

        public byte? DeviceType { get; set; }

        public string? DeviceToken { get; set; }

        public DateTime? ForgotPasswordDateUTC { get; set; }

        public DateTime AddedOnUTC { get; set; }

        public DateTime? UpdatedOnUTC { get; set; }
    }
}
