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
        public string Ma { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
    }

    public class QuestionPaginationModel : PaginationModel<QuestionModel>
    {
    }

    public class QuestionModel
    {
        public string Id { get; set; }

        public string Ma { get; set; }

        public string Content { get; set; }

        public string Type { get; set; }

        public int? Order { get; set; }
    }

    public class QuestionCreateModel
    {

        [Required(AllowEmptyStrings = false)]
        [OnlyNumber]
        [NotContainWhiteSpace]
        public string Ma { get; set; }

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
        [OnlyNumber]
        [NotContainWhiteSpace]
        public string Ma { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Type { get; set; }

        [Required]
        public int? Order { get; set; }
    }
}
