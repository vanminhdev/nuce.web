using nuce.web.api.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class ExamQuestions
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ExamQuestionsCreate
    {
        [Required(AllowEmptyStrings = false)]
        [NotContainWhiteSpace]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }
    }

    public class ExamStructure
    {
        public string Code { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public string QuestionId { get; set; }
    }

    public class AddQuestion
    {
        [Required]
        [NotContainWhiteSpace]
        public string ExamQuestionId { get; set; }

        [Required]
        public string QuestionCode { get; set; }

        [Required]
        public int? Order { get; set; }
    }
}
