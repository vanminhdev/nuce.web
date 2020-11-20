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

    public class SurveyRoundCreateModel : SurveyRoundUpdateModel
    {
    }

    public class SurveyRoundUpdateModel
    {
        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Name { get; set; }

        [Required]
        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        public DateTime? FromDate { get; private set; }

        public string FromDateString {
            set
            {
                try
                {
                    FromDate = DateTime.Parse(value);
                }
                catch
                {
                    FromDate = null;
                }
            }
        }

        [Required]
        [CompareMoreThanLessThan(true, "FromDate", ErrorMessage = "Đến ngày phải lớn hơn từ ngày")]
        public DateTime? EndDate { get; private set; }
        
        public string EndDateString
        {
            set
            {
                try
                {
                    EndDate = DateTime.Parse(value);
                }
                catch
                {
                    EndDate = null;
                }
            }
        }


        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false)]
        [NotOnlyContainWhiteSpace]
        public string Note { get; set; }

        [EnumDataType(typeof(SurveyRoundType))]
        public int Type { get; set; }
    }
}
