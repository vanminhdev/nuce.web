using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Attributes.ValidationAttributes
{
    /// <summary>
    /// Chuỗi không chỉ chứa toàn khoảng trắng thì thoả
    /// </summary>
    public class NotOnlyContainWhiteSpaceAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var str = value as string;
            if (str.Trim().Length > 0)
                return true;

            ErrorMessage = "Chuỗi không được chỉ chứa khoảng trắng";
            return false;
        }
    }
}
