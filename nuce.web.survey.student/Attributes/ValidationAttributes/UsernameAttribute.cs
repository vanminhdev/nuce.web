using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.survey.student.Attributes.ValidationAttributes
{
    /// <summary>
    /// Không chứa khoảng trắng và có dạng username ^[a-zA-Z0-9-._@+]+$, bỏ trống thì vẫn pass
    /// </summary>
    public class UsernameAttribute : RegularExpressionAttribute
    {
        public UsernameAttribute() : base(@"^[a-zA-Z0-9-._@+]+$")
        {
            ErrorMessage = "Tên tài khoản không hợp lệ";
        }
    }
}
