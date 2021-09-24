using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Undergraduate
{
    public class UndergraduateSurveyRoundFilter
    {
        public string Name { get; set; }
    }

    public class UndergraduateSurveyRoundCreate : UndergraduateSurveyRoundUpdate
    {
    }

    public class UndergraduateSurveyRoundUpdate
    {
        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Name { get; set; }

        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        public DateTime? FromDate { get; set; }

        [CompareMoreThanLessThan(true, "FromDate", ErrorMessage = "Đến ngày phải lớn hơn từ ngày")]
        public DateTime? EndDate { get; set; }
        
        public string Description { get; set; }

        public string Note { get; set; }
    }
}
