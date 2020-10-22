using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class Question
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Ma { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; }
        [Required(AllowEmptyStrings = false)]
        public int Order { get; set; }
    }
}
