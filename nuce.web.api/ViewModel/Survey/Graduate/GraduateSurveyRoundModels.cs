using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Graduate
{
    public class GraduateSurveyRoundFilter
    {
        public string Name { get; set; }
    }

    public class GraduateSurveyRoundCreate : GraduateSurveyRoundUpdate
    {
    }

    public class GraduateSurveyRoundUpdate
    {
        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Name { get; set; }

        [Required]
        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        public DateTime? FromDate { get; set; }

        [Required]
        [CompareMoreThanLessThan(true, "FromDate", ErrorMessage = "Đến ngày phải lớn hơn từ ngày")]
        public DateTime? EndDate { get; set; }
        
        public string Description { get; set; }

        public string Note { get; set; }
    }
}
