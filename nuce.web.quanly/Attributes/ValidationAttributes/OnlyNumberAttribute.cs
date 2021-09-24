using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Attributes.ValidationAttributes
{
    /// <summary>
    /// Phải là số
    /// </summary>
    public class OnlyNumberAttribute : RegularExpressionAttribute
    {
        public OnlyNumberAttribute() : base(@"^[0-9]*$")
        {
            ErrorMessage = "Phải là dạng số";
        }
    }
}
