using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class QuestionFilter
    {
        public string Code { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }

    public class QuestionModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public int? Order { get; set; }

        public string ParentCode { get; set; }

        public List<QuestionModel> QuestionChilds { get; set; }
    }

    public class QuestionCreateModel
    {

        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; }

        [Required]
        public int? Order { get; set; }
    }

    public class QuestionUpdateModel
    {

        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; }

        [Required]
        public int? Order { get; set; }

        public List<string> QuestionChildCodes { get; set; }
    }
}
