using nuce.web.api.Common;
using nuce.web.api.Models.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.Attributes.ValidationAttributes
{
    /// <summary>
    /// Không được bỏ trống và lựa chọn hợp lệ
    /// </summary>
    public class RolesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            NuceCoreIdentityContext context = (NuceCoreIdentityContext)validationContext.GetService(typeof(NuceCoreIdentityContext));

            var rolesCheck = context.Roles.Select(r => r.Id);
            var roles = value as List<string>;
            if(roles != null && roles.Count > 0)
            {
                foreach(var role in roles)
                {
                    if(!rolesCheck.Any(r => r == role))
                    {
                        return new ValidationResult("Role không chính xác");
                    }
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("Role không chính xác");
        }
    }
}
