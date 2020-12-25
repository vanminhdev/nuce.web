using nuce.web.api.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class AnswerModel
    {
        public string Id { get; set; }

        public string Code { get; set; }

        public string Content { get; set; }

        public int? Order { get; set; }

        public string CauHoiId { get; set; }

        public string CauHoiCode { get; set; }
    }

    public class AnswerCreateModel
    {
        //[Required(AllowEmptyStrings = false)]
        //[NotContainWhiteSpace]
        //public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required]
        public int? Order { get; set; }

        [Required]
        [NotContainWhiteSpace]
        public string CauHoiId { get; set; }

        [Required]
        [NotContainWhiteSpace]
        public string CauHoiCode { get; set; }

        [NotContainWhiteSpace]
        public string ChildQuestionId { get; set; }
    }

    public class AnswerUpdateModel
    {
        //[Required(AllowEmptyStrings = false)]
        //public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required]
        public int? Order { get; set; }
    }
}
