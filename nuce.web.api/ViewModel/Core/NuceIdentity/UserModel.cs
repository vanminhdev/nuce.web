using Microsoft.AspNetCore.Identity;
using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using nuce.web.api.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core.NuceIdentity
{
    public class UserCreateModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [EmailAddress]
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [RegularExpression("Admin|Department|Faculty", ErrorMessage = "Chọn 1 trong 3 vai trò Admin, Department, Faculty")]
        public string Role { get; set; }
    }

    public class UserModel {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
    }

    public class UserDetailModel : UserModel
    {
        public List<string> Roles { get; set; }
    }

    public class UserPaginationModel : PaginationModel<UserModel>
    {
    }

    public class UserFilter
    {
        public string Username { get; set; }
    }

    public class UserUpdateModel
    {
        [Required(AllowEmptyStrings = false)]
        public string UserName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        //[EnumDataType(typeof(UserStatus))]
        //public int? Status { get; set; }

        [Required]
        [Roles]
        public List<string> Roles { get; set; }
    }
}
