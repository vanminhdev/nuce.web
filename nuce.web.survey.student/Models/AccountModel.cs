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
    }
}