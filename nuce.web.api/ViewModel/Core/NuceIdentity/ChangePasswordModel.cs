using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Core.NuceIdentity
{
    public class ChangePasswordModel
    {

        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
