using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core.NuceIdentity
{
    public class LoginModel
    {
        [Required(AllowEmptyStrings = false)]
        [Username]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        public string Password { get; set; }

        public LoginUserType LoginUserType { get; set; }
    }
}
