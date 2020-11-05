using Microsoft.Ajax.Utilities;
using nuce.web.quanly.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.quanly.Models
{
    public class UserModel
    {
        public string id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public int status { get; set; }
    }

    public class UserUpdate
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Id không được để trống")]
        [NotContainWhiteSpace(ErrorMessage = "Id không được chứa khoảng trắng")]
        public string id { get; set; }

        //chỉ để xem
        public string username { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phoneNumber { get; set; }

        [EmailRegex(ErrorMessage = "Email không hợp lệ")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string email { get; set; }

        //[Required]
        //[EnumDataType(typeof(UserStatus))]
        //chỉ để xem
        public int status { get; set; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [Roles(ErrorMessage = "Vai trò không hợp lệ")]
        public List<string> roles { get; set; }
    }

    public class Password
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
        [NotContainWhiteSpace(ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string newPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Xác nhận mật khẩu mới không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới dài tối thiểu 6 kí tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu mới dài tối đa 30 kí tự")]
        [Compare("newPassword", ErrorMessage = "Xác nhận mật khẩu không khớp")]
        [NotContainWhiteSpace(ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string confirmPassword { get; set; }
    }

    public class UserDetail
    {
        public UserUpdate UserUpdateBind { get;set; }
        public Password ResetPassword { get; set; }
        public string ChangeStatus { get; set; }
    }

    public class UserCreate
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
        [MinLength(2, ErrorMessage = "Tên đăng nhập tối thiểu 1 ký tự")]
        [MaxLength(30, ErrorMessage = "Tên đăng nhập tối đa 30 kí tự")]
        [Username]
        public string username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 kí tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu tối đa 30 kí tự")]
        [NotContainWhiteSpace(ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string password { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phoneNumber { get; set; }

        [EmailRegex(ErrorMessage = "Email không hợp lệ")]
        [EmailAddress]
        public string email { get; set; }

        [Roles(ErrorMessage = "Vai trò không hợp lệ")]
        public List<string> roles { get; set; }
    }
}