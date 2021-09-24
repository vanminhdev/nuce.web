using nuce.web.quanly.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Attributes.ValidationAttributes
{
    /// <summary>
    /// Không được bỏ trống và lựa chọn hợp lệ
    /// </summary>
    public class RolesAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var rolesCheck = Definitions.Roles.Keys.ToList();
            var roles = value as List<string>;
            if(roles != null && roles.Count > 0)
            {
                foreach(var role in roles)
                {
                    if(!rolesCheck.Any(r => r == role))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
