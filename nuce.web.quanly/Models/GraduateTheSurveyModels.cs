using nuce.web.quanly.Attributes.ValidationAttributes;
using nuce.web.quanly.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.quanly.Models
{

    public class GraduateTheSurveyCreate : GraduateTheSurveyUpdate
    {

    }

    public class GraduateTheSurveyUpdate
    {
        [Required(AllowEmptyStrings = false)]
        public Guid? DotKhaoSatId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public Guid? DeThiId { get; set; }

        [Required]
        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        public DateTime? FromDate { get; private set; }
        public string FromDateString
        {
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

        [Required]
        [EnumDataType(typeof(GraduateSurveyRoundType))]
        public int? Type { get; set; }
    }
}
