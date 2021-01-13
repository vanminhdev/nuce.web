using nuce.web.api.Attributes.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace nuce.web.api.ViewModel.Survey
{
    public class ExamQuestionsFilter
    {
        public string Name { get; set; }
    }

    public class ExamQuestions
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ExamQuestionsCreate
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }
    }

    public class ExamStructure
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public int Order { get; set; }
        public Guid QuestionId { get; set; }
    }

    public class AddQuestion
    {
        [Required]
        [NotContainWhiteSpace]
        public Guid? ExamQuestionId { get; set; }

        [Required]
        public string QuestionCode { get; set; }

        [Required]
        public int? Order { get; set; }
    }

    public class SortQuestion
    {
        public Guid CauHoiId { get; set; }
        public int Order { get; set; }
    }

    public class GenerateExam
    {
        [Required]
        public Guid? ExamQuestionId { get; set; }

        public List<SortQuestion> SortResult { get; set; }
    }
}
