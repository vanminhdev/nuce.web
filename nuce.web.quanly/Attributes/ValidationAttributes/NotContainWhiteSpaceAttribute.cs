using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Attributes.ValidationAttributes
{
    /// <summary>
    /// Không chứa khoảng trắng, bỏ trống thì vẫn pass
    /// </summary>
    public class NotContainWhiteSpaceAttribute : RegularExpressionAttribute
    {
        public NotContainWhiteSpaceAttribute() : base(@"^[^\s]*")
        {
            ErrorMessage = "Không được chứa khoảng trắng";
        }
    }
}
