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

        public int? DapAnId { get; set; }

        public string Content { get; set; }

        public int? Order { get; set; }

        public string CauHoiGId { get; set; }

        public int? CauHoiId { get; set; }
    }

    public class AnswerCreateModel
    {
        [Required]
        public int? DapAnId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required]
        public int? Order { get; set; }

        [Required]
        public string CauHoiGId { get; set; }

        [Required]
        public int? CauHoiId { get; set; }
    }

    public class AnswerUpdateModel
    {
        [Required]
        public int? DapAnId { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required]
        public int? Order { get; set; }
    }
}
