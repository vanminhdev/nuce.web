using nuce.web.survey.student.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nuce.web.survey.student.Models
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã số sinh viên không được bỏ trống")]
        [NotContainWhiteSpace]
        [Username]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được bỏ trống")]
        [NotContainWhiteSpace]
        public string Password { get; set; }

        public int LoginUserType { get; set; }

        public string Type { get; set; }
    }

    public class LoginUserType
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class ChangePassword
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(6, ErrorMessage = "Mật khẩu dài ít nhất 6 ký tự")]
        [MaxLength(30, ErrorMessage = "Mật khẩu dài tối đa 30 ký tự")]
        [NotContainWhiteSpace(ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        public string NewPassword { get; set; }
    }
}