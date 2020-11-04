using nuce.web.quanly.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        [NotContainWhiteSpace]
        [Username]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [NotContainWhiteSpace]
        public string Password { get; set; }
    }

    public class Profile
    {
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phoneNumber { get; set; }

        [EmailRegex(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }
    }

    public class ChangePassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu cũ không được để trống")]
        [NotContainWhiteSpace]
        public string password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
        [NotContainWhiteSpace]
        public string newPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Xác nhận mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
        [Compare("newPassword", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        [NotContainWhiteSpace]
        public string confirmPassword { get; set; }
    }

    public class ProfileDetail
    {
        public Profile UpdateProfile { get; set; }

        public ChangePassword Password { get; set; }
    }
}