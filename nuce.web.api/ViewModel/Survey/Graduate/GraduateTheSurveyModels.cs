using nuce.web.api.Attributes.ValidationAttributes;
using nuce.web.api.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey.Graduate
{
    public class GraduateTheSurvey
    {
        public Guid Id { get; set; }
        public Guid DotKhaoSatId { get; set; }
        public Guid DeThiId { get; set; }
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }

        public string SurveyRoundName { get; set; }
    }

    public class GraduateTheSurveyFilter
    {
    }

    public class GraduateTheSurveyCreate : GraduateTheSurveyUpdate
    {

    }

    public class GraduateTheSurveyUpdate
    {
        [Required(AllowEmptyStrings = false)]
        public Guid? DotKhaoSatId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public Guid? DeThiId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Required]
        [CompareMoreThanLessThan(false, "EndDate", ErrorMessage = "Từ ngày phải nhỏ hơn đến ngày")]
        public DateTime? FromDate { get; set; }

        [Required]
        [CompareMoreThanLessThan(true, "FromDate", ErrorMessage = "Đến ngày phải lớn hơn từ ngày")]
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }

        public string Note { get; set; }

        [Required]
        [EnumDataType(typeof(TheSurveyType))]
        public int? Type { get; set; }
    }
}
