using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class SurveyRoundFilter
    {
        public string Name { get; set; }
    }

    public class SurveyRoundCreate : SurveyRoundUpdate
    {
    }

    public class SurveyRoundUpdate
    {
        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Name { get; set; }

        [Required]
        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        [DatetimeGreaterNow]
        public DateTime? FromDate { get; set; }

        [Required]
        [CompareMoreThanLessThan(true, "FromDate", ErrorMessage = "Đến ngày phải lớn hơn từ ngày")]
        [DatetimeGreaterNow]
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }
    }

    public class AddEndDate
    {
        [Required]
        public DateTime? EndDate { get; set; }
    }
}
