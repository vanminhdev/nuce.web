using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core.NuceIdentity
{
    public class RegisterModel
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
}
